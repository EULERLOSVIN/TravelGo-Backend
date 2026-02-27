using Application.DTOs.Vehicles;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly ApplicationDbContext _context; // cambia si tu DbContext se llama distinto

    public VehicleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<VehicleListItemDto>> GetVehiclesAsync()
    {
        var data = await _context.Vehicles
            .Include(v => v.IdVehicleStateNavigation)
            .Include(v => v.IdPersonNavigation)
            .Select(v => new VehicleListItemDto
            {
                UnitId = "U-" + v.IdVehicle.ToString("D3"),
                Plate = v.PlateNumber,
                Model = v.Model != null ? v.Model.ToString()! : "Sin modelo",

                // ⚠️ Ajusta nombres según tu tabla Person (si no es FirstName/LastName)
                Driver = v.IdPersonNavigation != null
                    ? (v.IdPersonNavigation.FirstName + " " + v.IdPersonNavigation.LastName)
                    : "Sin asignar",

                // ✅ 2 estados por Name
                IsActive = v.IdVehicleStateNavigation.Name.ToUpper() == "ACTIVO",

                // ✅ luego lo hacemos real con DocumentVehicle
                SoatOk = true
            })
            .ToListAsync();

        return data;
    }

    public async Task<VehicleSummaryDto> GetSummaryAsync()
    {
        var total = await _context.Vehicles.CountAsync();

        var active = await _context.Vehicles
            .Include(v => v.IdVehicleStateNavigation)
            .CountAsync(v => v.IdVehicleStateNavigation.Name.ToUpper() == "ACTIVO");

        var inactive = await _context.Vehicles
            .Include(v => v.IdVehicleStateNavigation)
            .CountAsync(v => v.IdVehicleStateNavigation.Name.ToUpper() == "INACTIVO");

        return new VehicleSummaryDto
        {
            TotalUnits = total,
            Active = active,
            Inactive = inactive
        };
    }
    public async Task<int> CreateVehicleAsync(CreateVehicleDto dto)
    {
        // 1️⃣ VEHICLE
        var vehicle = new Vehicle
        {
            PlateNumber = dto.Plate,
            Model = dto.Model,
            IdVehicleState = 1 // activo
        };

        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        // 2️⃣ DETAIL VEHICLE
        var detail = new DetailVehicle
        {
            IdVehicle = vehicle.IdVehicle,
            VehicleType = dto.Type,
            SeatNumber = dto.Seats
        };

        _context.DetailVehicles.Add(detail);

        // 3️⃣ DOCUMENT (SOAT)
        if (dto.SoatExpiry != null)
        {
            var document = new DocumentVehicle
            {
                IdVehicle = vehicle.IdVehicle,
                NumberSoat = "PENDING",
                ExpirationDate = dto.SoatExpiry.Value
            };

            _context.DocumentVehicles.Add(document);
        }

        // 4️⃣ ASSIGNMENT (driver)
        if (dto.DriverId != null)
        {
            var assign = new Assignment
            {
                IdAccount = dto.DriverId.Value,
                IdHeadquarter = 1,
                AssignmentDate = DateTime.UtcNow,
                IsActive = true
            };

            _context.Assignments.Add(assign);
        }

        await _context.SaveChangesAsync();

        return vehicle.IdVehicle;
    }
}
