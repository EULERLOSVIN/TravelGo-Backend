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

            // 2. Encuentra al Vehículo amarrado a esa Persona
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.IdPerson == person.IdPerson);
            if (vehicle == null) throw new Exception("Ese chofer no tiene un vehículo registrado.");

            // 3. Verifica si la Ruta a la que se intenta agregar es válida
            var route = await _context.TravelRoutes.FindAsync(dto.IdTravelRoute);
            if (route == null) throw new Exception("La ruta de destino no existe.");

            // 4. (Opcional) Revisar si el carro ya está en la cola para no duplicarlo, o si su VehicleState está disponible
            // var isAlreadyInQueue = await _context.AssignQueues.AnyAsync(aq => aq.IdVehicle == vehicle.IdVehicle);
            // if (isAlreadyInQueue) throw new Exception("Este chofer/vehículo ya se encuentra en cola.");

            // 5. Crea la instancia de la Cola (Generar el turno)
            // Por lógica del negocio, "Number" sería el siguiente turno consecutivo,
            // pero para abreviar lo creamos base y dejamos BD auto-increment.
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

            // 6. Crea la asociación entre el Turno Creado y el Carro + (Ruta: Asumiendo que va en RouteAssignment)
            var assignQueue = new AssignQueue
            {
                IdQueueVehicle = queueVehicle.IdQueueVehicle,
                IdVehicle = vehicle.IdVehicle
            };

            // NOTA: EL ER Model original no tiene "IdTravelRoute" en "AssignQueue". Si la Secretaria DEBE mandar la ruta,
            // debería modificarse AssignQueue para tener una Foreign Key hacia TravelRoute,
            // o crear/actualizar "RouteAssignment" de ese IdPerson hacia el dto.IdTravelRoute.
            // Lo dejaremos preparado como RouteAssignment para cumplir con tu diseño DB.
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
