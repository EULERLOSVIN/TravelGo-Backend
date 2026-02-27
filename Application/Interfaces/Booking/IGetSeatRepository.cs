

using Application.DTOs.Headquarters;

namespace Application.Interfaces.Booking
{
    public interface IGetSeatRepository
    {
        Task<List<SeatDto>> GetSeatByIdOfVehicle(int idSeatVehicle);
    }
}
