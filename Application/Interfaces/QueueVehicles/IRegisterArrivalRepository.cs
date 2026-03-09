using Application.Common;
using Application.Interfaces.QueueVehicles;

namespace Application.Interfaces.QueueVehicles
{
    public interface IRegisterArrivalRepository
    {
        Task<Result<bool>> RegisterArrivalAsync(string driverDni);
    }
}
