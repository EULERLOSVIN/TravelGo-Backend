using Application.DTOs.DepartureTimes;
using Application.Interfaces.DepartureTimes;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories.DepartureTimes
{
    public class GetDepartureTimesByRouteRepository : IGetDepartureTimesByRouteRepository
    {
        private readonly ApplicationDbContext _context;

        public GetDepartureTimesByRouteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DepartureTimeDto>> GetDepartureTimesByRouteAsync(int idTravelRoute)
        {
            // Fetch and order in DB, then map ToString in memory (EF Core can't translate TimeOnly.ToString to SQL)
            var entities = await _context.DepartureTimes
                .Where(dt => dt.IdTravelRoute == idTravelRoute)
                .OrderBy(dt => dt.Hour)
                .ToListAsync();

            return entities.Select(dt => new DepartureTimeDto
            {
                IdDepartureTime = dt.IdDepartureTime,
                IdTravelRoute = dt.IdTravelRoute,
                Hour = dt.Hour.ToString("HH:mm")
            }).ToList();
        }
    }
}

