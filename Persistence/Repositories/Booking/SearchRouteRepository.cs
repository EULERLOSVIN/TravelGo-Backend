
using Persistence.Context;

namespace Persistence.Repositories.Booking
{
    public class SearchRouteRepository
    {
        private readonly ApplicationDbContext _context;
        public SearchRouteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<> searchRouteByPlace()
        //{

        //}
    }
}
