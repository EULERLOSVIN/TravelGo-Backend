
using Application.DTOs.Driver;
using Application.Interfaces.Driver;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Driver
{
    public class TripsRepository: ITripsRepository
    {
        public readonly ApplicationDbContext _context;

        public TripsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TripsMadeDto>> GetTripsMade(int IdAccount, int daysBack)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.IdAccount == IdAccount);

            if (account == null) return new List<TripsMadeDto>();

            // OJO: Verifica si en tu DB es IdPerson o IdPersona, este es el error más común
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.IdPerson == account.IdPerson);

            if (vehicle == null) return new List<TripsMadeDto>();

            // 1. Calculamos el rango de fechas
            var today = DateOnly.FromDateTime(DateTime.Now);
            var startDate = today.AddDays(-daysBack); // Restamos los días que vienen del front

            // 2. Consulta con rango dinámico
            var trips = await _context.TravelTickets.Include(tt => tt.IdTravelRouteNavigation)
                .Where(tt => tt.IdVehicle == vehicle.IdVehicle
                          && tt.TravelDate >= startDate
                          && tt.TravelDate <= today) // Filtramos en el rango
                .GroupBy(tt => new {
                    tt.IdBilling,
                    tt.IdTravelRouteNavigation.NameRoute,
                    tt.TravelDate
                })
                .Select(g => new TripsMadeDto
                {
                    IdTrip = g.Key.IdBilling,
                    NameRoute = g.Key.NameRoute,
                    DepartureDate = g.Key.TravelDate,
                    NumberPassengers = g.Count()
                })
                .ToListAsync();

            return trips;
        }

        public async Task<StatisticsSummaryDriverDto> GetStatisticsSummaryForDriver(int IdAccount)
        {
            var trips = await GetTripsMade(IdAccount, 0);

            var dto = new StatisticsSummaryDriverDto
            {
                CompletedRaces = trips.Count,
                NumberPassengers = trips.Sum(trip => trip.NumberPassengers)
            };

            return dto;
        }
    }
}
