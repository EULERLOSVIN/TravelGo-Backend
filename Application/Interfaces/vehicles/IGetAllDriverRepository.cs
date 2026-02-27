using Application.DTOs.vehicles;

namespace Application.Interfaces.vehicles
{
    public interface IGetAllDriverRepository
    {
        Task<List<PersonDto>> GetDriversAsync();
    }
}
