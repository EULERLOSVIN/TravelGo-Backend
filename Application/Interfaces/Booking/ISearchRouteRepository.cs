
using Application.DTOs.Customers;

namespace Application.Interfaces.Booking
{
    public interface ISearchRouteRepository
    {
        Task<DataRouteDto?> SearchRouteByPlaceToday(SearchTravelRouteDto searchTravelDto);
        Task<DataRouteDto?> SearchRouteByPlaceTomorrow(SearchTravelRouteDto searchTravelRouteDto);
    }
}
