using Domain.Entities;
﻿using Application.DTOs.Vehicles;
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

    public async Task<bool> CreateVehicleAsync(CreateVehicleDto dto)
    {
        int driverId = (dto.IdPerson.HasValue && dto.IdPerson.Value > 0)
            ? dto.IdPerson.Value
            : 1; // 👈 Cambia por el Id real "Sin asignar"

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 1️⃣ VEHICLE
            var vehicle = new Vehicle
            {
                PlateNumber = dto.Plate.Trim(),
                Model = dto.Model,
                IdVehicleState = dto.IdVehicleState,
                IdPerson = driverId,
                Photo = dto.MainPhoto // 👈 Directo byte[]
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            // 2️⃣ DETAIL
            var detail = new DetailVehicle
            {
                IdVehicle = vehicle.IdVehicle,
                VehicleType = dto.VehicleType.Trim(),
                SeatNumber = dto.SeatNumber
            };

            _context.DetailVehicles.Add(detail);

            // 3️⃣ DOCUMENT
            var document = new DocumentVehicle
            {
                IdVehicle = vehicle.IdVehicle,
                ExpirationDate = dto.SoatExpiry
            };

            _context.DocumentVehicles.Add(document);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeleteVehicleAsync(string unitId)
{
    int idVehiculo = int.Parse(unitId.Replace("U-", ""));

    using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.IdVehicle == idVehiculo);

        if (vehicle == null)
            return false;

        var details = await _context.DetailVehicles
            .Where(d => d.IdVehicle == idVehiculo)
            .ToListAsync();

        var documents = await _context.DocumentVehicles
            .Where(d => d.IdVehicle == idVehiculo)
            .ToListAsync();

        if (documents.Any())
            _context.DocumentVehicles.RemoveRange(documents);

        if (details.Any())
            _context.DetailVehicles.RemoveRange(details);

        _context.Vehicles.Remove(vehicle);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return true;
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}

    public async Task<bool> UpdateVehicleAsync(string unitId, CreateVehicleDto dto)
    {
        int idVehiculo = int.Parse(unitId.Replace("U-", ""));

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.IdVehicle == idVehiculo);

            if (vehicle == null)
                return false;

            int driverId = (dto.IdPerson.HasValue && dto.IdPerson.Value > 0)
                ? dto.IdPerson.Value
                : 1;

            // VEHICLE
            vehicle.PlateNumber = dto.Plate.Trim();
            vehicle.Model = dto.Model;
            vehicle.IdVehicleState = dto.IdVehicleState;
            vehicle.IdPerson = driverId;

            if (dto.MainPhoto != null)
                vehicle.Photo = dto.MainPhoto;

            // DETAIL
            var detail = await _context.DetailVehicles
                .FirstOrDefaultAsync(d => d.IdVehicle == idVehiculo);

            if (detail != null)
            {
                detail.VehicleType = dto.VehicleType.Trim();
                detail.SeatNumber = dto.SeatNumber;
            }

            // DOCUMENT
            var document = await _context.DocumentVehicles
                .FirstOrDefaultAsync(d => d.IdVehicle == idVehiculo);

            if (document != null)
            {
                document.ExpirationDate = dto.SoatExpiry;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

}