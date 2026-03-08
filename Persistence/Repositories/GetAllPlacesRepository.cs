using Domain.Entities;
// places=darwin
using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class GetAllPlacesRepository : IGetAllPlacesRepository
    {
        private readonly ApplicationDbContext _context;
        public GetAllPlacesRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<PlaceDto>> GetAllPlaces()
        {
            return await _context.Places
                .Select(p => new PlaceDto
                {
                    idPlace = p.IdPlace,
                    name = p.Name,
                    description = p.Description
                })
                .ToListAsync();
        }
    }
}
