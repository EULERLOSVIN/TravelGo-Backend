using Application.DTOs.QueueVehicles;

namespace Application.Interfaces.QueueVehicles
{
    public interface IGetActiveQueueRepository
    {
        Task<List<QueueVehicleResponseDto>> GetActiveQueueAsync(int idHeadquarter);
    }
}
