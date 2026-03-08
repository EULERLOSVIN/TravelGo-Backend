using Domain.Entities;
// places=darwin
using Application.Interfaces;
using Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class DeletePlaceRepository : IDeletePlaceRepository
    {
        private readonly ApplicationDbContext _context;
        public DeletePlaceRepository(ApplicationDbContext context) => _context = context;

        public async Task<bool> DeletePlace(int idPlace)
        {
            var place = await _context.Places.FindAsync(idPlace);
            if (place == null) return false;

            try
            {
                _context.Places.Remove(place);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Manejo de restricción de llave foránea (Dependencias)
                return false;
            }
        }
    }
}
