using Application.Common;
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

        public async Task<Result<DriverQueueInfoDto>> GetDriverQueueInfoAsync(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni)) return Result<DriverQueueInfoDto>.Failure("DNI no proporcionado.");
            dni = dni.Trim();

            // First, find the driver by DNI
            var person = await _context.People.FirstOrDefaultAsync(p => p.NumberIdentityDocument == dni);
            if (person == null) return Result<DriverQueueInfoDto>.Failure("No se encontró ninguna persona con ese DNI.");

            // Build partial DTO with driver name
            var dto = new DriverQueueInfoDto
            {
                DriverFullName = $"{person.FirstName} {person.LastName}",
                DriverDni = person.NumberIdentityDocument ?? string.Empty,
                IdVehicle = 0,
                VehiclePlate = string.Empty,
                VehicleModel = string.Empty,
                AssignedRoutes = new List<DriverRouteDto>()
            };

            // Find their assigned vehicle
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.IdPerson == person.IdPerson);
            if (vehicle == null) return Result<DriverQueueInfoDto>.Success(dto);

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

            return Result<DriverQueueInfoDto>.Success(dto);
        }
    }
}
