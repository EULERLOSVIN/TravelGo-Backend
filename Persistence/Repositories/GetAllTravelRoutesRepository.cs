// rutas=darwin  Este código va a la base de datos, saca todas las rutas y las convierte en DTOs para enviarlas.
using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class GetAllTravelRoutesRepository : IGetAllTravelRoutesRepository
    {
        private readonly ApplicationDbContext _context;
        public GetAllTravelRoutesRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<TravelRouteDto>> GetAllTravelRoutes()
        {
            return await _context.TravelRoutes
                .Select(r => new TravelRouteDto
                {
                    idTravelRoute = r.IdTravelRoute,
                    nameRoute = r.NameRoute,
                    price = r.Price,
                    idPlaceA = r.IdPlaceA,
                    idPlaceB = r.IdPlaceB,
                    isActive = r.IsActive
                })
                .ToListAsync();
        }
    }
}
