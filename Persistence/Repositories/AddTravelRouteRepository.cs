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
            if (dto.idPlaceA == dto.idPlaceB)
            {
                throw new Exception("El origen y el destino no pueden ser el mismo lugar.");
            }

            // Buscar Place A (Origen)
            var placeA = await _context.Places.FindAsync(dto.idPlaceA);
            if (placeA == null) throw new Exception("El lugar de origen no existe");

            // Buscar Place B (Destino) 
            var placeB = await _context.Places.FindAsync(dto.idPlaceB);
            if (placeB == null) throw new Exception("El lugar de destino no existe");

            var route = new TravelRoute
            {
                NameRoute = $"{placeA.Name} - {placeB.Name}",
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
