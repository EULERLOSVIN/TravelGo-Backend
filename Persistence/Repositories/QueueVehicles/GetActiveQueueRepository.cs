using Domain.Entities;
using Application.DTOs.QueueVehicles;
using Application.Interfaces.QueueVehicles;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.QueueVehicles
{
    public class GetActiveQueueRepository : IGetActiveQueueRepository
    {
        private readonly ApplicationDbContext _context;

        public GetActiveQueueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<QueueVehicleResponseDto>> GetActiveQueueAsync(int idHeadquarter)
        {
            // Filtro maestro: Solo los vehiculos formados en cola 
            // que esten asignados a una ruta de esta Sede (idPlaceA == idHeadquarter).

            var queueData = await _context.AssignQueues
                .AsNoTracking()
                .Include(aq => aq.IdQueueVehicleNavigation)
                .Include(aq => aq.IdVehicleNavigation)
                    .ThenInclude(v => v.IdPersonNavigation)
                .Include(aq => aq.IdVehicleNavigation)
                    .ThenInclude(v => v.DetailVehicles)
                .Include(aq => aq.IdTravelRouteNavigation)
                    .ThenInclude(tr => tr.DepartureTimes)
                .Where(aq => aq.IdTravelRouteNavigation.IdPlaceA == idHeadquarter)
                .OrderBy(aq => aq.IdQueueVehicleNavigation.Number)
                .ToListAsync();

            var routeGroups = queueData.GroupBy(aq => aq.IdTravelRoute);
            var result = new List<QueueVehicleResponseDto>();
            var currentTime = TimeOnly.FromDateTime(DateTime.Now);

            foreach (var group in routeGroups)
            {
                var orderedQueue = group.OrderBy(aq => aq.IdQueueVehicleNavigation.Number).ToList();
                for (int i = 0; i < orderedQueue.Count; i++)
                {
                    var aq = orderedQueue[i];
                    var isEnTurno = i == 0;
                    var detailVehicle = aq.IdVehicleNavigation.DetailVehicles.FirstOrDefault();
                    var totalSeats = detailVehicle?.SeatNumber ?? 0;

                    int occupiedSeats = 0;
                    if (isEnTurno)
                    {
                        var today = DateOnly.FromDateTime(DateTime.Now);
                        occupiedSeats = await _context.TravelTickets
                            .CountAsync(t => t.IdVehicle == aq.IdVehicle && t.TravelDate == today);
                    }

                    string departureTimeStr = "--:--";
                    int remainingMins = 0;
                    var upcomingDeparture = aq.IdTravelRouteNavigation?.DepartureTimes
                        .Where(dt => dt.Hour > currentTime)
                        .OrderBy(dt => dt.Hour)
                        .Skip(i)
                        .FirstOrDefault();

                    if (upcomingDeparture != null)
                    {
                        departureTimeStr = upcomingDeparture.Hour.ToString("hh:mm tt");
                        remainingMins = (int)(upcomingDeparture.Hour.ToTimeSpan() - currentTime.ToTimeSpan()).TotalMinutes;
                    }

                    result.Add(new QueueVehicleResponseDto
                    {
                        IdAssignQueue = aq.IdAssignQueue,
                        Turn = aq.IdQueueVehicleNavigation.Number,
                        DriverFullName = aq.IdVehicleNavigation.IdPersonNavigation != null 
                            ? aq.IdVehicleNavigation.IdPersonNavigation.FirstName + " " + aq.IdVehicleNavigation.IdPersonNavigation.LastName 
                            : "S/N",
                        DriverDni = aq.IdVehicleNavigation.IdPersonNavigation?.NumberIdentityDocument ?? "S/N",
                        VehiclePlate = aq.IdVehicleNavigation.PlateNumber ?? "S/N",
                        VehicleModel = aq.IdVehicleNavigation.Model ?? "N/A",
                        IdRoute = aq.IdTravelRoute,
                        DestinationName = aq.IdTravelRouteNavigation?.NameRoute ?? "Sin Destino",
                        OccupiedSeats = occupiedSeats,
                        TotalSeats = totalSeats,
                        ScheduledDepartureTime = departureTimeStr,
                        RemainingMinutes = remainingMins,
                        Status = isEnTurno ? "EN TURNO" : "EN COLA"
                    });
                }
            }

            return result.OrderBy(r => r.Turn).ToList();
        }
    }
}
