using Application.DTOs.QueueVehicles;

namespace Application.Interfaces.QueueVehicles
{
    public interface IGetDriverQueueInfoRepository
    {
        Task<DriverQueueInfoDto?> GetDriverQueueInfoAsync(string dni);
    }
}
