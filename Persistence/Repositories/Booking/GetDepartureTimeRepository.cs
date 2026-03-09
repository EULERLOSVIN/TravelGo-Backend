using Application.DTOs.Customers;
using Application.Interfaces.Booking;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Booking
{
    public class GetDepartureTimeRepository: IGetDepartureTimeRepository
    {
        private readonly ApplicationDbContext _context;

        public GetDepartureTimeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // restantes
        public async Task<List<DepartureTimeDto>> GetRemainingDepartureTimes(int idRuta)
        {
            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);

            var hoursDeparture = await _context.DepartureTimes
                .Where(t => t.IdTravelRoute == idRuta && t.Hour >= currentTime)
                .OrderBy(t => t.Hour)
                .Select(t => new DepartureTimeDto { Id = t.IdDepartureTime, Hour = t.Hour })
                .ToListAsync();

            return hoursDeparture;
        }

        public async Task<List<DepartureTimeDto>> GetAllDepartureTimes(int idRuta)
        {
            var hoursDeparture = await _context.DepartureTimes
                .Where(t => t.IdTravelRoute == idRuta)
                .OrderBy(t => t.Hour)
                .Select(t => new DepartureTimeDto
                {
                    Id = t.IdDepartureTime,
                    Hour = t.Hour
                })
                .ToListAsync();

            return hoursDeparture;
        }
    }
}
