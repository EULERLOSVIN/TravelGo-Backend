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
                .Include(aq => aq.IdQueueVehicleNavigation)
                .Include(aq => aq.IdVehicleNavigation)
                    .ThenInclude(v => v.IdPersonNavigation)
                        .ThenInclude(p => p.RouteAssignments)
                            .ThenInclude(ra => ra.IdTravelRouteNavigation)
                .Where(aq => aq.IdVehicleNavigation.IdPersonNavigation.RouteAssignments
                              .Any(ra => ra.IdTravelRouteNavigation.IdPlaceA == idHeadquarter))
                .Select(aq => new QueueVehicleResponseDto
                {
                    IdAssignQueue = aq.IdAssignQueue,
                    Turn = aq.IdQueueVehicleNavigation.Number,
                    DriverFullName = aq.IdVehicleNavigation.IdPersonNavigation.FirstName + " " + aq.IdVehicleNavigation.IdPersonNavigation.LastName,
                    DriverDni = aq.IdVehicleNavigation.IdPersonNavigation.NumberIdentityDocument,
                    VehiclePlate = aq.IdVehicleNavigation.PlateNumber, // Ajustar nombre columna placa si LicensePlate no existe
                    VehicleModel = aq.IdVehicleNavigation.Model, // Ajustar nombre columna modelo
                    // Para simplificar, toma la primer ruta asignada activa de la sede.
                    IdRoute = aq.IdVehicleNavigation.IdPersonNavigation.RouteAssignments
                              .Where(ra => ra.IdTravelRouteNavigation.IdPlaceA == idHeadquarter)
                              .Select(ra => ra.IdTravelRoute)
                              .FirstOrDefault(),
                    DestinationName = aq.IdVehicleNavigation.IdPersonNavigation.RouteAssignments
                              .Where(ra => ra.IdTravelRouteNavigation.IdPlaceA == idHeadquarter)
                              .Select(ra => ra.IdTravelRouteNavigation.NameRoute)
                              .FirstOrDefault() ?? "Sin Destino"
                })
                .OrderBy(q => q.Turn)
                .ToListAsync();

            return queueData;
        }
    }
}
