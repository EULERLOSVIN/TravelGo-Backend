using Application.Features.QueueManagement.Queries.GetDriverQueueInfo;
using Application.Interfaces.QueueManagement;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.QueueManagement;

public class QueueManagementRepository : IQueueManagementRepository
{
    private readonly ApplicationDbContext _context;

    public QueueManagementRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetDriverQueueInfoResponse> GetDriverQueueInfoByDni(string dni)
    {
        var person = await _context.People
            .AsNoTracking()
            .Include(p => p.Vehicles)
            .Include(p => p.RouteAssignments)
                .ThenInclude(ra => ra.IdTravelRouteNavigation)
            .FirstOrDefaultAsync(p => p.NumberIdentityDocument == dni);

        if (person == null)
        {
            throw new KeyNotFoundException($"No se encontró personal con DNI: {dni}");
        }

        var vehicle = person.Vehicles.FirstOrDefault(); // Assuming one vehicle per driver for this module context

        var response = new GetDriverQueueInfoResponse
        {
            IdPerson = person.IdPerson,
            FullName = $"{person.FirstName} {person.LastName}",
            IdVehicle = vehicle?.IdVehicle ?? 0,
            PlateNumber = vehicle?.PlateNumber ?? "N/A",
            Model = vehicle?.Model ?? "N/A",
            AssignedRoutes = person.RouteAssignments
                .Select(ra => new RouteDto
                {
                    IdTravelRoute = ra.IdTravelRoute,
                    NameRoute = ra.IdTravelRouteNavigation?.NameRoute ?? "Desconocida"
                })
                .ToList()
        };

        return response;
    }

    public async Task<int> AddQueueVehicle(int idVehicle)
    {
        // 1. Get current max number
        var maxNumber = await _context.QueueVehicles
            .AsNoTracking()
            .MaxAsync(qv => (int?)qv.Number) ?? 0;

        // 2. Create QueueVehicle
        var queueVehicle = new Domain.Entities.QueueVehicle
        {
            Number = maxNumber + 1
        };

        _context.QueueVehicles.Add(queueVehicle);
        await _context.SaveChangesAsync();

        // 3. Create AssignQueue
        var assignQueue = new Domain.Entities.AssignQueue
        {
            IdQueueVehicle = queueVehicle.IdQueueVehicle,
            IdVehicle = idVehicle
        };

        _context.AssignQueues.Add(assignQueue);
        await _context.SaveChangesAsync();

        return queueVehicle.IdQueueVehicle;
    }

    public async Task<List<ActiveQueueDto>> GetActiveQueue()
    {
        var activeQueue = await _context.AssignQueues
            .AsNoTracking()
            .Include(aq => aq.IdQueueVehicleNavigation)
            .Include(aq => aq.IdVehicleNavigation)
                .ThenInclude(v => v.IdPersonNavigation)
            .Include(aq => aq.IdVehicleNavigation)
                .ThenInclude(v => v.RouteAssignments)
                    .ThenInclude(ra => ra.IdTravelRouteNavigation)
            .OrderBy(aq => aq.IdQueueVehicleNavigation.Number)
            .Select(aq => new ActiveQueueDto
            {
                IdQueueVehicle = aq.IdQueueVehicle,
                Number = aq.IdQueueVehicleNavigation.Number,
                PlateNumber = aq.IdVehicleNavigation.PlateNumber ?? "N/A",
                DriverName = aq.IdVehicleNavigation.IdPersonNavigation != null 
                             ? $"{aq.IdVehicleNavigation.IdPersonNavigation.FirstName} {aq.IdVehicleNavigation.IdPersonNavigation.LastName}" 
                             : "S/N",
                RouteName = aq.IdVehicleNavigation.RouteAssignments.Select(ra => ra.IdTravelRouteNavigation.NameRoute).FirstOrDefault() ?? "Sin Ruta",
                EntryTime = DateTime.Now // Placeholder since AssignQueue doesn't have CreatedAt yet
            })
            .ToListAsync();

        return activeQueue;
    }

    public async Task<bool> DeleteQueueVehicle(int idQueueVehicle)
    {
        var assignQueue = await _context.AssignQueues
            .FirstOrDefaultAsync(aq => aq.IdQueueVehicle == idQueueVehicle);

        if (assignQueue == null) return false;

        _context.AssignQueues.Remove(assignQueue);

        var queueVehicle = await _context.QueueVehicles
            .FirstOrDefaultAsync(qv => qv.IdQueueVehicle == idQueueVehicle);

        if (queueVehicle != null)
        {
            _context.QueueVehicles.Remove(queueVehicle);
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
