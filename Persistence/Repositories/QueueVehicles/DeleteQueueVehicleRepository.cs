using Domain.Entities;
using Application.Interfaces.QueueVehicles;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.QueueVehicles
{
    public class DeleteQueueVehicleRepository : IDeleteQueueVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        public DeleteQueueVehicleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteQueueVehicleAsync(int idAssignQueue)
        {
            var assignment = await _context.AssignQueues.FindAsync(idAssignQueue);
            if (assignment == null) return false;

            var queueItem = await _context.QueueVehicles.FindAsync(assignment.IdQueueVehicle);

            _context.AssignQueues.Remove(assignment);

            if (queueItem != null)
            {
                _context.QueueVehicles.Remove(queueItem);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
