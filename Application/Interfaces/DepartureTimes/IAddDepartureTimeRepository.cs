using Application.DTOs.DepartureTimes;
using System.Threading.Tasks;

namespace Application.Interfaces.DepartureTimes
{
    public interface IAddDepartureTimeRepository
    {
        Task<int> AddDepartureTimeAsync(AddDepartureTimeDto dto);
    }
}
