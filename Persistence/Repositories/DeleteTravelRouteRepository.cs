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
            var route = await _context.TravelRoutes.FindAsync(idTravelRoute);
            if (route == null) return false;

            try 
            {
                _context.TravelRoutes.Remove(route);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Probablemente restricción de llave foránea (la ruta tiene tickets o asignaciones)
                return false; 
            }
        }
    }
}
