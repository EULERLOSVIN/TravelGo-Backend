using Application.DTOs.DepartureTimes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.DepartureTimes
{
    public interface IGetDepartureTimesByRouteRepository
    {
        Task<List<DepartureTimeDto>> GetDepartureTimesByRouteAsync(int idTravelRoute);
    }
}
