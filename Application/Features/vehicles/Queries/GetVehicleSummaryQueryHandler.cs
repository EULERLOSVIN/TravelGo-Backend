//using Application.DTOs.Vehicles;
//using Application.Interfaces;
//using MediatR;

//namespace Application.Features.Vehicles.Queries;

//public class GetVehicleSummaryQueryHandler
//    : IRequestHandler<GetVehicleSummaryQuery, VehicleSummaryDto>
//{
//    private readonly IVehicleRepository _repository;

//    public GetVehicleSummaryQueryHandler(IVehicleRepository repository)
//    {
//        _repository = repository;
//    }

//    public async Task<VehicleSummaryDto> Handle(
//        GetVehicleSummaryQuery request,
//        CancellationToken cancellationToken)
//    {
//        return await _repository.GetSummaryAsync();
//    }
//}
