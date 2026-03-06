using Domain.Entities;
using Application.DTOs.DepartureTimes;
using Application.Interfaces.DepartureTimes;
using Persistence.Context;
using System.Threading.Tasks;

namespace Persistence.Repositories.DepartureTimes
{
    public class AddDepartureTimeRepository : IAddDepartureTimeRepository
    {
        private readonly ApplicationDbContext _context;

        public AddDepartureTimeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddDepartureTimeAsync(AddDepartureTimeDto dto)
        {
            var entity = new DepartureTime
            {
                IdTravelRoute = dto.IdTravelRoute,
                Hour = TimeOnly.Parse(dto.Hour) // Convert "HH:mm:ss" string to TimeOnly
            };

            await _context.DepartureTimes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdDepartureTime;
        }
    }
}
