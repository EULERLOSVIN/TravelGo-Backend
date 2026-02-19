using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Booking
{
    public interface IBookingRepository
    {
        Task<bool> SelectSeat(int idSeatVehicle);
        Task<bool> ConfirmPayment(int idSeatVehicle);
        Task<bool> ReleaseSeat(int idSeatVehicle);
        Task ReleaseSeatIfPending(int idSeatVehicle);
        Task<bool> UpdatePassengerDetails(int idSeatVehicle, string dni, string fullName, string pickUp);
    }
}
