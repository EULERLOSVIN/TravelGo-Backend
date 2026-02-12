// rutas=darwin
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IUpdateTravelRouteRepository
    {
        Task<bool> UpdateTravelRoute(UpdateTravelRouteDto dto);
    }
}
