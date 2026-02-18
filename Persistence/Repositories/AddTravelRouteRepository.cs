using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class AddTravelRouteRepository : IAddTravelRouteRepository
    {
        private readonly ApplicationDbContext _context;
        public AddTravelRouteRepository(ApplicationDbContext context) => _context = context;


        public async Task<int> AddTravelRoute(AddTravelRouteDto dto)
        {
            var parts = dto.nameRoute.Split('-');
            string originName = parts.Length > 0 ? parts[0].Trim() : "Origen Desconocido";
            string destinationName = parts.Length > 1 ? parts[1].Trim() : "Destino Desconocido";

            var placeA = await _context.Places.FirstOrDefaultAsync(p => p.Name == originName);
            if (placeA == null)
            {
                placeA = new Place { Name = originName, Description = "Creado automáticamente" };
                _context.Places.Add(placeA);
                await _context.SaveChangesAsync();
            }

            var placeB = await _context.Places.FirstOrDefaultAsync(p => p.Name == destinationName);
            if (placeB == null)
            {
                placeB = new Place { Name = destinationName, Description = "Creado automáticamente" };
                _context.Places.Add(placeB);
                await _context.SaveChangesAsync();
            }

            var route = new TravelRoute
            {
                NameRoute = dto.nameRoute,
                Price = dto.price,
                IdPlaceA = placeA.IdPlace,
                IdPlaceB = placeB.IdPlace
            };

            _context.TravelRoutes.Add(route);
            await _context.SaveChangesAsync();
            return route.IdTravelRoute;
        }
    }
}
