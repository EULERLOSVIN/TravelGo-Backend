using Application.Common;
using Application.Interfaces.QueueVehicles;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.QueueVehicles
{
    public class RegisterArrivalRepository : IRegisterArrivalRepository
    {
        private readonly ApplicationDbContext _context;

        public RegisterArrivalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<bool>> RegisterArrivalAsync(string driverDni)
        {
            // 1. Find the person by DNI
            var person = await _context.People
                .FirstOrDefaultAsync(p => p.NumberIdentityDocument == driverDni);
            if (person == null)
                return Result<bool>.Failure("Chofer no encontrado. Verifique el DNI ingresado.");

            // 2. Find their vehicle
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.IdPerson == person.IdPerson);
            if (vehicle == null)
                return Result<bool>.Failure("Este chofer no tiene un vehículo asignado.");

            // 3. Find their most recent active trip (IdStateTrip = 1 = "En Curso")
            var activeTrip = await _context.Trips
                .Where(t => t.IdVehicle == vehicle.IdVehicle && t.IdStateTrip == 1)
                .OrderByDescending(t => t.DepartureDate)
                .FirstOrDefaultAsync();

            if (activeTrip == null)
                return Result<bool>.Failure("Este chofer no tiene un viaje en curso actualmente.");

            // 4. Register arrival: set ArrivalDate and change state to "Finalizado" (2)
            activeTrip.ArrivalDate = DateTime.Now;
            activeTrip.IdStateTrip = 2;

            await _context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
