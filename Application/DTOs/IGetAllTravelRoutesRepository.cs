// rutas=darwin   Define que existe una acción para "Obtener Todas las Rutas".
using Application.DTOs;
namespace Application.Interfaces
{
    public interface IGetAllTravelRoutesRepository
    {
        Task<List<TravelRouteDto>> GetAllTravelRoutes();
    }
}