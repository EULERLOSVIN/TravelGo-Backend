using System.Threading.Tasks;

namespace Application.Interfaces.DepartureTimes
{
    public interface IDeleteDepartureTimeRepository
    {
        Task<bool> DeleteDepartureTimeAsync(int idDepartureTime);
    }
}
