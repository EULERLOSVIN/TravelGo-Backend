using Application.Common;
using Application.DTOs.QueueVehicles;

namespace Application.Interfaces.QueueVehicles
{
    public interface IGetDriverQueueInfoRepository
    {
        Task<Result<DriverQueueInfoDto>> GetDriverQueueInfoAsync(string dni);
    }
}
