

using Application.DTOs.ManageSales;
using Application.Interfaces.Routes;
using Persistence.Context;

namespace Persistence.Repositories.Routes
{
    public class RouteRepository: IRouteRepository
    {
        private readonly ApplicationDbContext _context;

        public RouteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RouteDto>> GetAllRoutes()
        {
            var routes = _context.TravelRoutes
                .Select(x => new RouteDto { IdRoute = x.IdTravelRoute, Name = x.NameRoute })
                .ToList();

            return routes;
        }
    }
}
