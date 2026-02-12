// rutas=darwin  Define que existe una acción para "Agregar Ruta".
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IAddTravelRouteRepository
    {
        Task<int> AddTravelRoute(AddTravelRouteDto dto);
    }
}
