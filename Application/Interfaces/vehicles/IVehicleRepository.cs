using Application.DTOs.Vehicles;

namespace Application.Interfaces;

public interface IVehicleRepository
{
    Task<bool> CreateVehicleAsync(CreateVehicleDto dto);
    Task<List<VehicleListItemDto>> GetVehiclesAsync();
    Task<VehicleSummaryDto> GetSummaryAsync();
    Task<bool> UpdateVehicleAsync(string unitId, CreateVehicleDto dto);
    Task<bool> DeleteVehicleAsync(string unitId);
}
