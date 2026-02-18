
using Application.DTOs.Customers;

namespace Application.Interfaces.Customers
{
    public interface IGetAllPlaceofRouteRepository
    {
        Task<List<GetPlaceDto>?> GetAllPlaceofRoute();
    }
}
