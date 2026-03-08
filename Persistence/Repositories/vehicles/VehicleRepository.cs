using Domain.Entities;
using Application.DTOs.Vehicles;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Application.DTOs.vehicles;

namespace Persistence.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly ApplicationDbContext _context;

    public VehicleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<StateVehicleDto>?> GetStateVehicle()
    {
        return await _context.VehicleStates
        .Select(sv => new StateVehicleDto
        {
            IdState = sv.IdVehicleState,
            NameState = sv.Name
        })
        .ToListAsync();
    }

    public async Task<bool> RegisterVehicle(CreateVehicleDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            byte[]? photoBytes = null;
            if (!string.IsNullOrEmpty(dto.MainPhoto))
            {
                // Limpiamos el string por si Angular incluye el prefijo "data:image/..."
                string base64Data = dto.MainPhoto.Contains(",")
                                    ? dto.MainPhoto.Split(',')[1]
                                    : dto.MainPhoto;

                // Convertimos a array de bytes
                photoBytes = Convert.FromBase64String(base64Data);
            }
            var vehicle = new Vehicle
            {
                IdVehicleState = dto.IdState,
                IdPerson = dto.IdDriver,
                PlateNumber = dto.Plate,
                Model = dto.Model,
                Photo = photoBytes
            };
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            var detailVehicle = new DetailVehicle
            {
                IdVehicle = vehicle.IdVehicle,
                SeatNumber = dto.SeatNumber,
                VehicleType = dto.VehicleType
            };
            _context.DetailVehicles.Add(detailVehicle);

            var documentVehicle = new DocumentVehicle
            {
                IdVehicle = vehicle.IdVehicle,
                SoatExpirationDate = dto.SoatExpirationDate
            };
            _context.DocumentVehicles.Add(documentVehicle);

            var seatsToAssign = await _context.Seats
                .OrderBy(s => s.Number)
                .Take(dto.SeatNumber)
                .ToListAsync();

            if (seatsToAssign.Count < dto.SeatNumber)
            {
                throw new Exception("No hay suficientes asientos definidos.");
            }

            var seatVehicles = seatsToAssign.Select(s => new SeatVehicle
            {
                IdVehicle = vehicle.IdVehicle,
                IdSeat = s.IdSeat,
                StateSeat = true
            }).ToList();

            _context.SeatVehicles.AddRange(seatVehicles);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Error al registrar vehículo: {ex.Message}");
            return false;
        }
    }

    public async Task<List<DetailVehicleDto>> GetVehiclesByFilters(FilterVehicleDto Filters)
    {
        const int pageSize = 5;
        int pageNumber = Filters.PageNumber ?? 1;

        if (pageNumber < 1) pageNumber = 1;
        var query = _context.Vehicles
            .AsNoTracking()
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(Filters.SearchTerm))
        {
            var term = Filters.SearchTerm.Trim();
            query = query.Where(v =>
            v.IdPersonNavigation.FirstName.Contains(term) ||
            v.IdPersonNavigation.LastName.Contains(term) ||
            v.PlateNumber.Contains(term) ||
            v.IdPersonNavigation.RouteAssignments.Any(ra => ra.IdTravelRouteNavigation.NameRoute.Contains(term))
            );
        }

        return await query
            .OrderBy(v => v.IdPersonNavigation.LastName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(v => new DetailVehicleDto
            {
                IdVehicle = v.IdVehicle,
                PhotoBase64 = v.Photo != null ? Convert.ToBase64String(v.Photo) : null,
                Type = v.DetailVehicles.FirstOrDefault() != null
                ? v.DetailVehicles.FirstOrDefault().VehicleType: "No definido",
                Plate = v.PlateNumber,
                Route = v.IdPersonNavigation.RouteAssignments.FirstOrDefault() != null
                ? v.IdPersonNavigation.RouteAssignments.FirstOrDefault().IdTravelRouteNavigation.NameRoute: "Sin ruta",
                Driver = v.IdPersonNavigation.FirstName + " " + v.IdPersonNavigation.LastName,
                State = v.IdVehicleStateNavigation.Name,
                ExpirationSoatDate = v.DocumentVehicles.FirstOrDefault() != null
                         ? v.DocumentVehicles.FirstOrDefault().SoatExpirationDate
                         : default(DateOnly)
            }).ToListAsync();
    }
}

