

using Application.DTOs.Customers;

namespace Application.Interfaces.Booking
{
    public interface IGetDepartureTimeRepository
    {
        Task<List<DepartureTimeDto>> GetRemainingDepartureTimes(int idRuta);
        Task<List<DepartureTimeDto>> GetAllDepartureTimes(int idRuta);
    }
}
