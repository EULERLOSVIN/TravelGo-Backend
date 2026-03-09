using Application.Common;
using Application.DTOs.QueueVehicles;
using Application.Interfaces.QueueVehicles;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using MediatR;
using Application.Features.QueueVehicles.Commands.DispatchVehicle;

namespace Persistence.Repositories.QueueVehicles
{
    public class GetActiveQueueRepository : IGetActiveQueueRepository
    {
        private readonly ApplicationDbContext _context;

        public GetActiveQueueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<QueueVehicleResponseDto>>> GetActiveQueueAsync(int idRoute, bool isArrival = false)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            
            return isArrival 
                ? await GetEnRouteArrivalsAsync(idRoute, today) 
                : await GetWaitingDepartureQueueAsync(idRoute, today);
        }

        private async Task<Result<List<QueueVehicleResponseDto>>> GetWaitingDepartureQueueAsync(int idRoute, DateOnly today)
        {
            var currentTime = TimeOnly.FromDateTime(DateTime.Now);

            // 1. Get all vehicles currently assigned to the wait list for this route
            var queueData = await _context.AssignQueues
                .AsNoTracking()
                .Include(aq => aq.IdVehicleNavigation).ThenInclude(v => v.IdPersonNavigation)
                .Include(aq => aq.IdVehicleNavigation).ThenInclude(v => v.DetailVehicles)
                .Include(aq => aq.IdTravelRouteNavigation).ThenInclude(tr => tr.DepartureTimes)
                .Include(aq => aq.IdQueueVehicleNavigation)
                .Where(aq => aq.IdTravelRoute == idRoute)
                .OrderBy(aq => aq.IdQueueVehicleNavigation.Number)
                .ToListAsync();

            if (!queueData.Any()) return Result<List<QueueVehicleResponseDto>>.Success(new List<QueueVehicleResponseDto>());

            // 2. DISABLED: SAFETY CHECK: Exclude "Zombie Trips"
            // The user wants to see their vehicles in the queue even if they have an old active trip pending arrival.
            /*
            var activeVehicleIdsWithTrips = await _context.Trips
                .Where(t => t.IdStateTrip == 1)
                .Select(t => t.IdVehicle)
                .ToListAsync();

            var filteredQueue = queueData.Where(aq => !activeVehicleIdsWithTrips.Contains(aq.IdVehicle)).ToList();
            */
            var filteredQueue = queueData;

            // 3. OPTIMIZATION: Bulk fetch ticket counts
            var vehicleIds = filteredQueue.Select(aq => aq.IdVehicle).Distinct().ToList();
            var ticketCounts = await GetTicketCountsAsync(vehicleIds, idRoute, today);

            // 4. MAPPING & SCHEDULE ROLLOVER (STRICTLY SEQUENTIAL)
            var allSchedules = filteredQueue.FirstOrDefault()?.IdTravelRouteNavigation?.DepartureTimes
                .OrderBy(dt => dt.Hour)
                .ToList() ?? new List<Persistence.DepartureTime>();

            var futureTimes = allSchedules
                .Where(dt => dt.Hour > currentTime)
                .ToList();

            // Create a pool that starts with today's remaining times, then follows with tomorrow's full schedule
            var resultPool = new List<Persistence.DepartureTime>();
            resultPool.AddRange(futureTimes);
            
            // Fill enough for the current queue using the full schedule (tomorrow, etc.)
            while (resultPool.Count < filteredQueue.Count && allSchedules.Any())
            {
                resultPool.AddRange(allSchedules);
            }

            var result = new List<QueueVehicleResponseDto>();
            for (int i = 0; i < filteredQueue.Count; i++)
            {
                var aq = filteredQueue[i];
                var detail = aq.IdVehicleNavigation.DetailVehicles.FirstOrDefault();
                var driver = aq.IdVehicleNavigation.IdPersonNavigation;
                
                // Assign schedules sequentially from the pool (never repeating the same time for different ranks)
                var upcoming = resultPool.ElementAtOrDefault(i);
                
                var occupiedSeats = ticketCounts.GetValueOrDefault(aq.IdVehicle, 0);

                result.Add(MapToResponseDto(aq, i + 1, driver, detail, upcoming, occupiedSeats, false));
            }

            return Result<List<QueueVehicleResponseDto>>.Success(result);
        }

        private async Task<Result<List<QueueVehicleResponseDto>>> GetEnRouteArrivalsAsync(int idRoute, DateOnly today)
        {
            // 1. Get all active trips (IdStateTrip == 1)
            // CLEAN LOGIC: A vehicle is "En Ruta" to this specific route ONLY if they have tickets sold for it today.
            // We NO LONGER rely on RouteAssignments which are volatile.
            var activeTrips = await _context.Trips
                .AsNoTracking()
                .Include(t => t.IdVehicleNavigation).ThenInclude(v => v.IdPersonNavigation)
                .Include(t => t.IdVehicleNavigation).ThenInclude(v => v.DetailVehicles)
                .Where(t => t.IdStateTrip == 1)
                .ToListAsync();

            if (!activeTrips.Any()) return Result<List<QueueVehicleResponseDto>>.Success(new List<QueueVehicleResponseDto>());

            // 2. Filter trips that actually belong to this route based on tickets
            var vehicleIds = activeTrips.Select(t => t.IdVehicle).Distinct().ToList();
            var ticketCounts = await GetTicketCountsAsync(vehicleIds, idRoute, today);
            
            // Only keep trips for which we found tickets for this specific route
            var tripsInRoute = activeTrips.Where(t => ticketCounts.ContainsKey(t.IdVehicle)).ToList();

            // 3. MAPPING
            var result = tripsInRoute.Select(trip => 
            {
                var driver = trip.IdVehicleNavigation.IdPersonNavigation;
                var detail = trip.IdVehicleNavigation.DetailVehicles.FirstOrDefault();
                var occupiedSeats = ticketCounts.GetValueOrDefault(trip.IdVehicle, 0);

                return MapToResponseDto(trip, 0, driver, detail, null, occupiedSeats, true);
            }).ToList();

            return Result<List<QueueVehicleResponseDto>>.Success(result);
        }

        private async Task<Dictionary<int, int>> GetTicketCountsAsync(List<int> vehicleIds, int idRoute, DateOnly today)
        {
            return await _context.TravelTickets
                .AsNoTracking()
                .Where(tt => vehicleIds.Contains(tt.IdVehicle) && 
                              tt.IdTravelRoute == idRoute && 
                              tt.TravelDate == today &&
                              tt.IdTicketState == 1)
                .GroupBy(tt => tt.IdVehicle)
                .Select(g => new { IdVehicle = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.IdVehicle, x => x.Count);
        }

        private QueueVehicleResponseDto MapToResponseDto(object source, int turn, Persistence.Person? driver, Persistence.DetailVehicle? detail, Persistence.DepartureTime? time, int occupiedSeats, bool isArrival)
        {
            if (source is AssignQueue aq)
            {
                return new QueueVehicleResponseDto
                {
                    IdAssignQueue = aq.IdAssignQueue,
                    Turn = turn,
                    DriverFullName = driver != null ? $"{driver.FirstName} {driver.LastName}" : "S/N",
                    DriverDni = driver?.NumberIdentityDocument ?? "S/N",
                    VehiclePlate = aq.IdVehicleNavigation.PlateNumber,
                    VehicleModel = aq.IdVehicleNavigation.Model,
                    IdRoute = aq.IdTravelRoute,
                    DestinationName = aq.IdTravelRouteNavigation?.NameRoute ?? "S/D",
                    OccupiedSeats = occupiedSeats,
                    TotalSeats = detail?.SeatNumber ?? 0,
                    ScheduledDepartureTime = time?.Hour.ToString("HH:mm") ?? "--:--",
                    RemainingMinutes = time != null ? (int)(time.Hour.ToTimeSpan() - TimeOnly.FromDateTime(DateTime.Now).ToTimeSpan()).TotalMinutes : 0,
                    Status = turn == 1 ? "EN TURNO" : "EN COLA",
                    IsArrival = false
                };
            }
            else if (source is Trip trip)
            {
                return new QueueVehicleResponseDto
                {
                    IdAssignQueue = 0,
                    Turn = 0,
                    DriverFullName = driver != null ? $"{driver.FirstName} {driver.LastName}" : "S/N",
                    DriverDni = driver?.NumberIdentityDocument ?? "S/N",
                    VehiclePlate = trip.IdVehicleNavigation.PlateNumber,
                    VehicleModel = trip.IdVehicleNavigation.Model,
                    IdRoute = 0, // In route, the specific route ID is inferred from context
                    DestinationName = "EN CAMINO",
                    OccupiedSeats = occupiedSeats,
                    TotalSeats = detail?.SeatNumber ?? 0,
                    ScheduledDepartureTime = trip.DepartureDate?.ToString("HH:mm") ?? "--:--",
                    RemainingMinutes = 0,
                    Status = "EN RUTA",
                    IsArrival = true
                };
            }
            return new QueueVehicleResponseDto();
        }
    }
}
