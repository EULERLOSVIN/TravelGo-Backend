

using Application.Interfaces.Driver;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.Driver
{
    public class StartingOrderRepository: IStartingOrderRepository
    {
        public readonly ApplicationDbContext _context;
        public StartingOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetStartingOrderByDriver(int idAccount)
        {
            // 1. Obtener la cuenta
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.IdAccount == idAccount);
            if (account == null) return 0;

            // 2. Obtener el vehículo usando el IdPerson de la cuenta
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.IdPerson == account.IdPerson);
            if (vehicle == null) return 0;

            // 3. Obtener la cola incluyendo la navegación (CRUCIAL)
            var assignmentQueue = await _context.AssignQueues
                .Include(aq => aq.IdQueueVehicleNavigation) // <--- ESTO ES LO QUE TE FALTA
                .FirstOrDefaultAsync(a => a.IdVehicle == vehicle.IdVehicle);

            // 4. Retornar con seguridad de nulos (? y ??)
            return assignmentQueue?.IdQueueVehicleNavigation?.Number ?? 0;
        }
    }
}
