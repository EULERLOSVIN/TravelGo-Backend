// rutas=darwin
namespace Application.Interfaces
{
    public interface IDeleteTravelRouteRepository
    {
        Task<bool> DeleteTravelRoute(int idTravelRoute);
    }
}
