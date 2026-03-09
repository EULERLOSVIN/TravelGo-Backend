using Application.DTOs.vehicles;
using Application.DTOs.Vehicles;

namespace Application.Interfaces;

public interface IVehicleRepository
{
    Task<bool> RegisterVehicle(CreateVehicleDto dto);
    Task<List<DetailVehicleDto>> GetVehiclesByFilters(FilterVehicleDto Filters);
    Task<List<StateVehicleDto>?> GetStateVehicle();
    Task<SummaryStatisticalOfVehicleDto> GetStatisticalSummaryOfVehicles();
    Task<bool> EditVehicle(EditVehicleDto newData);
}
