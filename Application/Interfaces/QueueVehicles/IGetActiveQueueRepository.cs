using Application.Common;
using Application.DTOs.QueueVehicles;

namespace Application.Interfaces.QueueVehicles
{
    public interface IGetActiveQueueRepository
    {
        Task<Result<List<QueueVehicleResponseDto>>> GetActiveQueueAsync(int idRoute, bool IsArrival = false);
    }
}
