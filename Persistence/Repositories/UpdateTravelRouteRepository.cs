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
            var route = await _context.TravelRoutes.FindAsync(dto.idTravelRoute);
            if (route == null) return false;

            // Parsear "Origen - Destino" del nombre
            var parts = dto.nameRoute.Split('-');
            string originName = parts.Length > 0 ? parts[0].Trim() : "Origen Desconocido";
            string destinationName = parts.Length > 1 ? parts[1].Trim() : "Destino Desconocido";

            // Buscar o crear Place A (Origen)
            var placeA = await _context.Places.FirstOrDefaultAsync(p => p.Name == originName);
            if (placeA == null)
            {
                placeA = new Place { Name = originName, Description = "Creado automáticamente" };
                _context.Places.Add(placeA);
                await _context.SaveChangesAsync();
            }

            // Buscar o crear Place B (Destino)
            var placeB = await _context.Places.FirstOrDefaultAsync(p => p.Name == destinationName);
            if (placeB == null)
            {
                placeB = new Place { Name = destinationName, Description = "Creado automáticamente" };
                _context.Places.Add(placeB);
                await _context.SaveChangesAsync();
            }

            route.NameRoute = dto.nameRoute;
            route.Price = dto.price;
            route.IdPlaceA = placeA.IdPlace;
            route.IdPlaceB = placeB.IdPlace;

            _context.TravelRoutes.Update(route);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
