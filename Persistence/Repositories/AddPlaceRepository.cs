using Application.DTOs;
using Application.Interfaces;
using Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class AddPlaceRepository : IAddPlaceRepository // Implementación de la interfaz para agregar un nuevo lugar
    {
        private readonly ApplicationDbContext _context;
        public AddPlaceRepository(ApplicationDbContext context) => _context = context;

        public async Task<int> AddPlace(AddPlaceDto dto) // Devuelve el Id del nuevo lugar
        {
            // Validar que no exista un lugar con el mismo nombre (insensible a mayúsculas)
            var exists = await _context.Places
                .AnyAsync(p => p.Name.ToLower() == dto.name.ToLower());
            
            if (exists)
            {
                throw new Exception($"El lugar '{dto.name}' ya existe.");
            }

            var place = new Place
            {
                Name = dto.name,
                Description = dto.description ?? string.Empty
            };
            
            _context.Places.Add(place);
            await _context.SaveChangesAsync();
            return place.IdPlace;
        }
    }
}
