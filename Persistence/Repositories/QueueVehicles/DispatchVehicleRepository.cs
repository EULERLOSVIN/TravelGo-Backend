using Application.Common;
using Application.Interfaces.QueueVehicles;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.QueueVehicles
{
    public class DispatchVehicleRepository : IDispatchVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        public DispatchVehicleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> DispatchVehicleAsync(int idAssignQueue)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Get the queue entry
                var assignQueue = await _context.AssignQueues
                    .Include(aq => aq.IdQueueVehicleNavigation)
                    .FirstOrDefaultAsync(aq => aq.IdAssignQueue == idAssignQueue);

                if (assignQueue == null)
                {
                    return Result<int>.Failure("No se encontró el registro en la cola.");
                }

                int idVehicle = assignQueue.IdVehicle;

                // 2. Insert into Trip (State 1 = En Curso)
                var newTrip = new Trip
                {
                    IdVehicle = idVehicle,
                    IdStateTrip = 1, // En Curso
                    DepartureDate = DateTime.Now
                };

                _context.Trips.Add(newTrip);
                await _context.SaveChangesAsync();

                // 3. Remove from AssignQueue
                _context.AssignQueues.Remove(assignQueue);
                
                // 4. (Optional) If this was the last one in QueueVehicle, we might want to clean up QueueVehicle 
                // but usually QueueVehicle is just a header. Let's just remove the assignment.
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Result<int>.Success(newTrip.IdTrip);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result<int>.Failure($"Error al despachar el vehículo: {ex.Message}");
            }
        }
    }
}
