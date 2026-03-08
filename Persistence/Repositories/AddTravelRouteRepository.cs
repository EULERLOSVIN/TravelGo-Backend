using Domain.Entities;
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
            if (dto == null) throw new ArgumentNullException(nameof(dto), "El DTO no puede ser nulo");

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
                IdPlaceA = dto.idPlaceA,
                IdPlaceB = dto.idPlaceB,
                Price = dto.price,
                NameRoute = $"{placeA.Name} - {placeB.Name}",
                IsActive = dto.isActive // Mapear estado
            };

            _context.TravelRoutes.Add(route);
            await _context.SaveChangesAsync();
            return route.IdTravelRoute;
        }
    }
}
