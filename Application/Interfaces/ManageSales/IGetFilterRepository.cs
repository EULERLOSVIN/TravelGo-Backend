

using Application.DTOs.ManageSales;

namespace Application.Interfaces.ManageSales
{
    public interface IGetFilterRepository
    {
        Task<FiltersDto> GetFilterForMageSales();
    }
}
