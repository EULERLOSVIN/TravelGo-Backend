// rutas=darwin
using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class UpdateTravelRouteRepository : IUpdateTravelRouteRepository
    {
        private readonly ApplicationDbContext _context;
        public UpdateTravelRouteRepository(ApplicationDbContext context) => _context = context;

        public async Task<bool> UpdateTravelRoute(UpdateTravelRouteDto dto)
        {
            if (dto.idPlaceA == dto.idPlaceB)
            {
                return false; // O lanzar excepción
            }

            var route = await _context.TravelRoutes.FindAsync(dto.idTravelRoute);
            if (route == null) return false;

            // Buscar Place A (Origen)
            var placeA = await _context.Places.FindAsync(dto.idPlaceA);
            if (placeA == null) return false;

            // Buscar Place B (Destino)
            var placeB = await _context.Places.FindAsync(dto.idPlaceB);
            if (placeB == null) return false;

            route.IdPlaceA = dto.idPlaceA;
            route.IdPlaceB = dto.idPlaceB;
            route.Price = dto.price;
            route.NameRoute = $"{placeA.Name} - {placeB.Name}";

            _context.TravelRoutes.Update(route);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
