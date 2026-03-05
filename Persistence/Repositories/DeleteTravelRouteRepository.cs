using Application.Interfaces;
using Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class DeleteTravelRouteRepository : IDeleteTravelRouteRepository
    {
        private readonly ApplicationDbContext _context;
        public DeleteTravelRouteRepository(ApplicationDbContext context) => _context = context;

        public async Task<bool> DeleteTravelRoute(int idTravelRoute)
        {
            // 1. Buscar la ruta con sus dependencias de configuración
            var route = await _context.TravelRoutes
                .Include(r => r.DepartureTimes)
                    .ThenInclude(dt => dt.QueueVehicles)
                .Include(r => r.RouteAssignments)
                .FirstOrDefaultAsync(r => r.IdTravelRoute == idTravelRoute);

            if (route == null) return false;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try 
            {
                // 2. Limpiar dependencias de colas y horarios (Configuración, no ticket)
                foreach (var dt in route.DepartureTimes)
                {
                    foreach (var qv in dt.QueueVehicles)
                    {
                        var assignQueues = _context.AssignQueues.Where(aq => aq.IdQueueVehicle == qv.IdQueueVehicle);
                        _context.AssignQueues.RemoveRange(assignQueues);
                    }
                    _context.QueueVehicles.RemoveRange(dt.QueueVehicles);
                }
                _context.DepartureTimes.RemoveRange(route.DepartureTimes);
                _context.RouteAssignments.RemoveRange(route.RouteAssignments);

                await _context.SaveChangesAsync();

                // 3. Intentar borrado FÍSICO de la Ruta
                _context.TravelRoutes.Remove(route);
                await _context.SaveChangesAsync();
                
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

                // 4. Si falla (probablemente por Tickets / Historial Real), hacemos SOFT DELETE
                var routeToDeactivate = await _context.TravelRoutes.FindAsync(idTravelRoute);
                if (routeToDeactivate != null)
                {
                    routeToDeactivate.IsActive = false;
                    _context.TravelRoutes.Update(routeToDeactivate);
                    await _context.SaveChangesAsync();
                }
                return true; 
            }
        }
    }
}
