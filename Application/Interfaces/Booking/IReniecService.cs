using Application.Common;
using Application.DTOs.Customers;

namespace Application.Interfaces.Booking
{
    public interface IReniecService
    {
        Task<PersonApiDto?> ConsultByDni(int dni);
    }
}
