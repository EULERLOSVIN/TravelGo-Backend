


using Application.DTOs.ManageSales;

namespace Application.Interfaces.Routes
{
    public interface IRouteRepository
    {
        Task<List<RouteDto>> GetAllRoutes();
    }
}
