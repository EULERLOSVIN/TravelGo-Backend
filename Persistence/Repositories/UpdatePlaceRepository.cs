using Domain.Entities;
using Application.DTOs;
using Application.Interfaces;
using Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class UpdatePlaceRepository : IUpdatePlaceRepository
    {
        private readonly ApplicationDbContext _context;
        public UpdatePlaceRepository(ApplicationDbContext context) => _context = context;

        public async Task<bool> UpdatePlace(UpdatePlaceDto dto)
        {
            var place = await _context.Places.FindAsync(dto.idPlace);
            if (place == null) return false;

            // Validar duplicados (otro lugar con el mismo nombre)
            var exists = await _context.Places
                .AnyAsync(p => p.Name.ToLower() == dto.name.ToLower() && p.IdPlace != dto.idPlace);

            if (exists) return false; // O lanzar excepción si prefieres

            place.Name = dto.name;
            place.Description = dto.description ?? string.Empty;

            _context.Places.Update(place);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
