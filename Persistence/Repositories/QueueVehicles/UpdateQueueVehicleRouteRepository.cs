using Domain.Entities;
using Application.DTOs.QueueVehicles;
using Application.Interfaces.QueueVehicles;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.QueueVehicles
{
    public class UpdateQueueVehicleRouteRepository : IUpdateQueueVehicleRouteRepository
    {
        private readonly ApplicationDbContext _context;

        public UpdateQueueVehicleRouteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateQueueVehicleRouteAsync(UpdateRouteQueueVehicleDto dto)
        {
            // Ubica la asignacion en cola
            var assignment = await _context.AssignQueues
                .Include(aq => aq.IdVehicleNavigation)
                .FirstOrDefaultAsync(aq => aq.IdAssignQueue == dto.IdAssignQueue);

            if (assignment == null) throw new Exception("El elemento no existe en la cola.");

            // En este Entity Modelo, la ruta se aloja en RouteAssignment apuntando a IdPerson.
            var personId = assignment.IdVehicleNavigation.IdPerson;
            var routeAssignment = await _context.RouteAssignments
                .FirstOrDefaultAsync(r => r.IdPerson == personId);

            if (routeAssignment != null)
            {
                routeAssignment.IdTravelRoute = dto.NewIdTravelRoute;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
