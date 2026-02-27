using Application.DTOs.Vehicles;

namespace Application.Interfaces;

public interface IVehicleRepository
{
    Task<List<VehicleListItemDto>> GetVehiclesAsync();
    Task<VehicleSummaryDto> GetSummaryAsync();
    Task<int> CreateVehicleAsync(CreateVehicleDto dto);
}
