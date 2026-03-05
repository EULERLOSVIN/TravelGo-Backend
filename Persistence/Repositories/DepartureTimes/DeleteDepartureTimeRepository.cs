using Application.Interfaces.DepartureTimes;
using Persistence.Context;
using System.Threading.Tasks;

namespace Persistence.Repositories.DepartureTimes
{
    public class DeleteDepartureTimeRepository : IDeleteDepartureTimeRepository
    {
        private readonly ApplicationDbContext _context;

        public DeleteDepartureTimeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteDepartureTimeAsync(int idDepartureTime)
        {
            var entity = await _context.DepartureTimes.FindAsync(idDepartureTime);
            if (entity == null) return false;

            _context.DepartureTimes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
