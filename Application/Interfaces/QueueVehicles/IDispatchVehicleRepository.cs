using Application.Common;

namespace Application.Interfaces.QueueVehicles
{
    public interface IDispatchVehicleRepository
    {
        Task<Result<int>> DispatchVehicleAsync(int idAssignQueue);
    }
}
