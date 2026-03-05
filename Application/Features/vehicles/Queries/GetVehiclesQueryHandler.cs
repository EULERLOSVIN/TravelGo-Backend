using Application.DTOs.Vehicles;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Vehicles.Queries;

public class GetVehiclesQueryHandler
    : IRequestHandler<GetVehiclesQuery, List<VehicleListItemDto>>
{
    private readonly IVehicleRepository _repository;

    public GetVehiclesQueryHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<VehicleListItemDto>> Handle(
        GetVehiclesQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetVehiclesAsync();
    }
}
