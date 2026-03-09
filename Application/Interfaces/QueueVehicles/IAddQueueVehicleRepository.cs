using Application.Common;
using Application.DTOs.QueueVehicles;

namespace Application.Interfaces.QueueVehicles
{
    public interface IAddQueueVehicleRepository
    {
        Task<Result<int>> AddQueueVehicleAsync(AddQueueVehicleDto dto);
    }
}
