using Application.Common;
using Application.DTOs;
using Application.Interfaces.QueueVehicles;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Text;
using System.Linq;

namespace Persistence.Repositories.QueueVehicles
{
    public class GetRoutesByHeadquarterRepository : IGetRoutesByHeadquarterRepository
    {
        private readonly ApplicationDbContext _context;

        public GetRoutesByHeadquarterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TravelRouteDto>> GetRoutesByHeadquarterAsync(int idHeadquarter, string type)
        {
            var h = await _context.Headquarters.FindAsync(idHeadquarter);
            if (h == null) return new List<TravelRouteDto>();

            var places = await _context.Places.AsNoTracking().ToListAsync();
            var hqPlaceIds = GetMatchingPlaceIds(h, places);

            if (!hqPlaceIds.Any()) return new List<TravelRouteDto>();

            var isArrival = type.ToLower() == "arrival";

            // STRICT FILTERING: 
            // Departure -> Route must START at this HQ (IdPlaceA)
            // Arrival (En Ruta) -> Route must END at this HQ (IdPlaceB)
            var travelRoutesQuery = _context.TravelRoutes
                .AsNoTracking()
                .Include(r => r.IdPlaceANavigation)
                .Include(r => r.IdPlaceBNavigation)
                .Where(r => r.IsActive);

            if (isArrival)
            {
                travelRoutesQuery = travelRoutesQuery.Where(r => hqPlaceIds.Contains(r.IdPlaceB));
            }
            else
            {
                travelRoutesQuery = travelRoutesQuery.Where(r => hqPlaceIds.Contains(r.IdPlaceA));
            }

            var travelRoutes = await travelRoutesQuery.ToListAsync();

            var result = new List<TravelRouteDto>();

            foreach (var r in travelRoutes)
            {
                var displayName = isArrival 
                    ? $"Desde {r.IdPlaceANavigation?.Name ?? "Origen"}" 
                    : $"Hacia {r.IdPlaceBNavigation?.Name ?? "Destino"}";

                int count;
                if (isArrival)
                {
                    // RESTORE STABLE LOGIC: Link Trip -> Vehicle -> Person -> RouteAssignment -> Route
                    // This identifies vehicles currently "En Ruta" that belong to this route's assignment.
                    count = await _context.Trips
                        .AsNoTracking()
                        .CountAsync(t => t.IdStateTrip == 1 && 
                                        _context.RouteAssignments.Any(ra => ra.IdTravelRoute == r.IdTravelRoute && 
                                                                           _context.Vehicles.Any(v => v.IdVehicle == t.IdVehicle && v.IdPerson == ra.IdPerson)));
                }
                else
                {
                    count = await _context.AssignQueues
                        .AsNoTracking()
                        .CountAsync(aq => aq.IdTravelRoute == r.IdTravelRoute);
                }

                result.Add(new TravelRouteDto
                {
                    idTravelRoute = r.IdTravelRoute,
                    nameRoute = displayName,
                    price = r.Price,
                    idPlaceA = r.IdPlaceA,
                    idPlaceB = r.IdPlaceB,
                    isActive = r.IsActive,
                    inQueueCount = count
                });
            }

            return result;
        }

        private List<int> GetMatchingPlaceIds(Headquarter h, List<Place> places)
        {
            string Normalize(string text)
            {
                if (string.IsNullOrWhiteSpace(text)) return "";
                var normalized = text.ToLower()
                    .Replace("sede", "").Replace("central", "").Replace("principal", "").Replace("prinsipal", "")
                    .TrimEnd('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ').Trim();

                if (string.IsNullOrEmpty(normalized)) return "";

                string formD = normalized.Normalize(NormalizationForm.FormD);
                var sb = new StringBuilder();
                foreach (char c in formD)
                {
                    var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                    if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                    {
                        sb.Append(c);
                    }
                }
                return sb.ToString().Normalize(NormalizationForm.FormC);
            }

            var nName = Normalize(h.Name);
            var nDist = Normalize(h.District);
            var nProv = Normalize(h.Province);

            // If all identifiers are empty after normalization (e.g., "Sede Central 1"), 
            // we MUST NOT match everything. We should return an empty list or fall back to a specific logic.
            if (string.IsNullOrEmpty(nName) && string.IsNullOrEmpty(nDist) && string.IsNullOrEmpty(nProv))
            {
                return new List<int>();
            }

            return places
                .Where(p => {
                    var pName = Normalize(p.Name);
                    if (string.IsNullOrEmpty(pName)) return false;

                    // Priority matching: 
                    // 1. Exact match on District/Province is strongest (if not empty)
                    // 2. Exact match on Name (after removing "Sede")
                    // Avoid .Contains() which causes "Lima" to match "Tingo Maria - Lima"
                    if (nDist != "" && pName == nDist) return true;
                    if (nProv != "" && pName == nProv) return true;
                    if (nName != "" && pName == nName) return true;
                    
                    return false;
                })
                .Select(p => p.IdPlace)
                .ToList();
        }
    }
}
