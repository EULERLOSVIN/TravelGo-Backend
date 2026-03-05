
using Application.DTOs.Driver;

namespace Application.Interfaces.Driver
{
    public interface ITripsRepository
    {
        Task<List<TripsMadeDto>> GetTripsMade(int IdAccount, int filterOption);
        Task<StatisticsSummaryDriverDto> GetStatisticsSummaryForDriver(int IdAccount);
    }
}
