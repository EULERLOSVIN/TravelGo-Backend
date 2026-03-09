using Domain.Entities;
using Application.DTOs.QueueVehicles;
using Application.Interfaces.QueueVehicles;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.QueueVehicles
{
    public class AddQueueVehicleRepository : IAddQueueVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        public AddQueueVehicleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddQueueVehicleAsync(AddQueueVehicleDto dto)
        {
            // 1. Encuentra a la Persona por el string DriverDni proveído en el DTO
            var person = await _context.People.FirstOrDefaultAsync(p => p.NumberIdentityDocument == dto.DriverDni);
            if (person == null) throw new Exception("Chofer no encontrado por DNI.");

            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.IdPerson == person.IdPerson);
            if (vehicle == null) throw new Exception("Ese chofer no tiene un vehículo registrado.");

            var route = await _context.TravelRoutes.FindAsync(dto.IdTravelRoute);
            if (route == null) throw new Exception("La ruta de destino no existe.");

            var newTurn = 1;
            var lastQueuePushed = await _context.QueueVehicles.OrderByDescending(q => q.IdQueueVehicle).FirstOrDefaultAsync();
            if (lastQueuePushed != null) {
                newTurn = lastQueuePushed.Number + 1;
            }

            var queueVehicle = new QueueVehicle
            {
                Number = newTurn,
                IdDepartureTime = dto.IdDepartureTime
            };

            await _context.QueueVehicles.AddAsync(queueVehicle);
            await _context.SaveChangesAsync(); // Para conseguir el IdQueueVehicle

            var assignQueue = new AssignQueue
            {
                IdQueueVehicle = queueVehicle.IdQueueVehicle,
                IdVehicle = vehicle.IdVehicle,
                IdTravelRoute = dto.IdTravelRoute
            };

            var existingRouteAssignment = await _context.RouteAssignments
                .FirstOrDefaultAsync(r => r.IdPerson == person.IdPerson);

            if (existingRouteAssignment != null) {
                existingRouteAssignment.IdTravelRoute = dto.IdTravelRoute;
            } else {
                var newRouteAssignment = new RouteAssignment {
                    IdPerson = person.IdPerson,
                    IdTravelRoute = dto.IdTravelRoute
                };
                await _context.RouteAssignments.AddAsync(newRouteAssignment);
            }

            await _context.AssignQueues.AddAsync(assignQueue);
            await _context.SaveChangesAsync();

            return queueVehicle.IdQueueVehicle;
        }
    }
}
