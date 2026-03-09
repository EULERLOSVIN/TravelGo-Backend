using Application.DTOs;

namespace Application.Interfaces.QueueVehicles
{
    public interface IGetRoutesByHeadquarterRepository
    {
        Task<List<TravelRouteDto>> GetRoutesByHeadquarterAsync(int idHeadquarter, string type);
    }
}
