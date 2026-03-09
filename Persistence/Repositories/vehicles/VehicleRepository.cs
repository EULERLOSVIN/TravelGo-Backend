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
            // 1. Procesamiento de imagen
            byte[]? photoBytes = null;
            if (!string.IsNullOrEmpty(dto.MainPhoto))
            {
                string base64Data = dto.MainPhoto.Contains(",")
                                    ? dto.MainPhoto.Split(',')[1]
                                    : dto.MainPhoto;
                photoBytes = Convert.FromBase64String(base64Data);
            }

            // 2. Registro del Vehículo
            var vehicle = new Vehicle
            {
                IdVehicleState = dto.IdState,
                IdPerson = dto.IdDriver, // Relación directa con el conductor
                PlateNumber = dto.Plate,
                Model = dto.Model,
                Photo = photoBytes
            };
            _context.Vehicles.Add(vehicle);
            // Guardamos para obtener el vehicle.IdVehicle generado
            await _context.SaveChangesAsync();

            // 3. Registro de Detalles del Vehículo
            var detailVehicle = new DetailVehicle
            {
                IdVehicle = vehicle.IdVehicle,
                SeatNumber = dto.SeatNumber,
                VehicleType = dto.VehicleType
            };
            _context.DetailVehicles.Add(detailVehicle);

            // 4. Registro de Documentos (SOAT)
            var documentVehicle = new DocumentVehicle
            {
                IdVehicle = vehicle.IdVehicle,
                SoatExpirationDate = dto.SoatExpirationDate
            };
            _context.DocumentVehicles.Add(documentVehicle);

            // 5. Asignación de Ruta (¡ESTO ES LO QUE FALTABA!)
            // Como tu SQL buscaba aquí la ruta, debemos insertar este registro
            var routeAssignment = new RouteAssignment
            {
                IdPerson = dto.IdDriver,
                IdTravelRoute = dto.IdRoute,
                AssignmentDate = DateOnly.FromDateTime(DateTime.Now)
            };
            _context.RouteAssignments.Add(routeAssignment);

            // 6. Configuración de Asientos
            var seatsToAssign = await _context.Seats
                .OrderBy(s => s.Number)
                .Take(dto.SeatNumber)
                .ToListAsync();

            if (seatsToAssign.Count < dto.SeatNumber)
            {
                throw new Exception("No hay suficientes asientos definidos en el sistema.");
            }

            var seatVehicles = seatsToAssign.Select(s => new SeatVehicle
            {
                IdVehicle = vehicle.IdVehicle,
                IdSeat = s.IdSeat,
                StateSeat = true
            }).ToList();

            _context.SeatVehicles.AddRange(seatVehicles);

            // 7. Guardar todo y confirmar
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
                Plate = v.PlateNumber,

                Type = v.DetailVehicles.FirstOrDefault() != null
        ? v.DetailVehicles.FirstOrDefault().VehicleType
        : "No definido",

                SeatNumber = v.DetailVehicles.FirstOrDefault() != null
        ? v.DetailVehicles.FirstOrDefault().SeatNumber
        : 0,

                Model = v.Model,

                Route = v.IdPersonNavigation.RouteAssignments.FirstOrDefault() != null
        ? v.IdPersonNavigation.RouteAssignments.FirstOrDefault().IdTravelRouteNavigation.NameRoute
        : "Sin ruta",

                IdRoute = v.IdPersonNavigation.RouteAssignments.FirstOrDefault() != null
        ? v.IdPersonNavigation.RouteAssignments.FirstOrDefault().IdTravelRoute
        : 0,  // 👈 nuevo

                Driver = v.IdPersonNavigation.FirstName + " " + v.IdPersonNavigation.LastName,

                IdDriver = v.IdPersonNavigation.IdPerson,  // 👈 nuevo

                State = v.IdVehicleStateNavigation.Name,

                IdState = v.IdVehicleStateNavigation.IdVehicleState, // 👈 nuevo

                ExpirationSoatDate = v.DocumentVehicles.FirstOrDefault() != null
        ? v.DocumentVehicles.FirstOrDefault().SoatExpirationDate
        : default
            }).ToListAsync();
    }

    public async Task<SummaryStatisticalOfVehicleDto> GetStatisticalSummaryOfVehicles()
    {
        var TotalVehicles = _context.Vehicles.Count();
        var TotalActiveVehicles = _context.Vehicles.Where(v => v.IdVehicleState == 1).Count();
        var TotalInactiveVehicles = _context.Vehicles.Where(v => v.IdVehicleState == 2).Count();

        return new SummaryStatisticalOfVehicleDto
        {
            TotalVehicles = TotalVehicles,
            TotalActiveVehicles = TotalActiveVehicles,
            TotalInactiveVehicles = TotalInactiveVehicles
        };
    }

    public async Task<bool> EditVehicle(EditVehicleDto newData)
    {
        // 1. Buscamos el vehículo incluyendo las colecciones
        var vehicle = await _context.Vehicles
            .Include(v => v.DetailVehicles)
            .Include(v => v.DocumentVehicles)
            .Include(v => v.SeatVehicles)
            .FirstOrDefaultAsync(v => v.IdVehicle == newData.IdVehicle);

        if (vehicle == null) return false;

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 2. Actualizamos campos principales
            vehicle.PlateNumber = newData.Plate;
            vehicle.Model = newData.Model ?? vehicle.Model;
            vehicle.IdPerson = newData.IdDriver;
            vehicle.IdVehicleState = newData.IdState;

            if (!string.IsNullOrEmpty(newData.MainPhoto))
            {
                string base64Data = newData.MainPhoto.Contains(",") ? newData.MainPhoto.Split(',')[1] : newData.MainPhoto;
                vehicle.Photo = Convert.FromBase64String(base64Data);
            }

            // 3. Actualizamos colecciones (DetailVehicle y DocumentVehicles)
            var detail = vehicle.DetailVehicles.FirstOrDefault();
            if (detail != null)
            {
                if (detail.SeatNumber != newData.SeatNumber)
                {
                    detail.SeatNumber = newData.SeatNumber;
                    _context.SeatVehicles.RemoveRange(vehicle.SeatVehicles);

                    var newSeats = await _context.Seats
                        .OrderBy(s => s.Number)
                        .Take(newData.SeatNumber)
                        .ToListAsync();

                    var newSeatVehicles = newSeats.Select(s => new SeatVehicle
                    {
                        IdVehicle = vehicle.IdVehicle,
                        IdSeat = s.IdSeat,
                        StateSeat = true
                    }).ToList();

                    _context.SeatVehicles.AddRange(newSeatVehicles);
                }
                detail.VehicleType = newData.VehicleType;
            }

            var document = vehicle.DocumentVehicles.FirstOrDefault();
            if (document != null)
            {
                document.SoatExpirationDate = newData.SoatExpirationDate;
            }

            // 4. MANEJO DE RUTA (UPSERT: Update o Insert)
            // Buscamos si el conductor ya tiene una asignación de ruta
            var assignment = await _context.RouteAssignments
                .FirstOrDefaultAsync(r => r.IdPerson == newData.IdDriver);

            if (assignment != null)
            {
                // Si ya existe, actualizamos la ruta
                assignment.IdTravelRoute = newData.IdRoute;
                assignment.AssignmentDate = DateOnly.FromDateTime(DateTime.Now);
            }
            else
            {
                // Si no existe, creamos una nueva
                var newAssignment = new RouteAssignment
                {
                    IdPerson = newData.IdDriver,
                    IdTravelRoute = newData.IdRoute,
                    AssignmentDate = DateOnly.FromDateTime(DateTime.Now)
                };
                _context.RouteAssignments.Add(newAssignment);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Error al editar vehículo: {ex.Message}");
            return false;
        }
    }
}

