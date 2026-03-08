using Application.DTOs.QueueVehicles;

namespace Application.Interfaces.QueueVehicles
{
    public interface IUpdateQueueVehicleRouteRepository
    {
        Task<bool> UpdateQueueVehicleRouteAsync(UpdateRouteQueueVehicleDto dto);
    }
}
