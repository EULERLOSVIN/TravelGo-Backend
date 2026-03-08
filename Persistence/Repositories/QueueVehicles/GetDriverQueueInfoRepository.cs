using Application.DTOs.QueueVehicles;
using Application.Interfaces.QueueVehicles;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.QueueVehicles
{
    public class GetDriverQueueInfoRepository : IGetDriverQueueInfoRepository
    {
        private readonly ApplicationDbContext _context;

        public GetDriverQueueInfoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DriverQueueInfoDto?> GetDriverQueueInfoAsync(string dni)
        {
            // First, find the driver by DNI - only return null if driver truly doesn't exist
            var person = await _context.People.FirstOrDefaultAsync(p => p.NumberIdentityDocument == dni);
            if (person == null) return null; // DNI not found -> 404

            // Security check: only allow drivers (role = "Chofer") to be found
            var account = await _context.Accounts
                .Include(a => a.IdRoleNavigation)
                .FirstOrDefaultAsync(a => a.IdPerson == person.IdPerson);

            if (account == null || account.IdRoleNavigation.Name != "Chofer")
                return null; // Non-driver roles are treated as not found

            // Build partial DTO with driver name (always returned when person exists)
            var dto = new DriverQueueInfoDto
            {
                DriverFullName = $"{person.FirstName} {person.LastName}",
                DriverDni = person.NumberIdentityDocument ?? string.Empty,
                IdVehicle = 0,
                VehiclePlate = string.Empty,
                VehicleModel = string.Empty,
                AssignedRoutes = new List<DriverRouteDto>()
            };

            // Find their assigned vehicle (optional - driver might not have one yet)
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.IdPerson == person.IdPerson);
            if (vehicle == null) return dto; // Driver exists but no vehicle -> return with empty routes

            dto.IdVehicle = vehicle.IdVehicle;
            dto.VehiclePlate = vehicle.PlateNumber ?? string.Empty;
            dto.VehicleModel = vehicle.Model ?? string.Empty;

            // Find their assigned routes (via RouteAssignment)
            dto.AssignedRoutes = await _context.RouteAssignments
                .Include(r => r.IdTravelRouteNavigation)
                .Where(r => r.IdPerson == person.IdPerson)
                .Select(r => new DriverRouteDto
                {
                    IdRoute = r.IdTravelRoute,
                    DestinationName = r.IdTravelRouteNavigation != null ? r.IdTravelRouteNavigation.NameRoute : "Desconocido"
                })
                .ToListAsync();

            return dto;
        }
    }
}
