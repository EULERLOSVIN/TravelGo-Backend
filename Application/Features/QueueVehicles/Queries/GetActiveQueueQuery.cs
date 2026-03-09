using System.Collections.Generic;
using Application.Common;
using Application.DTOs.QueueVehicles;
using Application.Interfaces.QueueVehicles;
using MediatR;

namespace Application.Features.QueueVehicles.Queries
{
    public record GetActiveQueueQuery(int IdHeadquarter, int IdRoute, bool IsArrival) : IRequest<Result<List<QueueVehicleResponseDto>>>;

    public class GetActiveQueueHandler(IGetActiveQueueRepository repository) : IRequestHandler<GetActiveQueueQuery, Result<List<QueueVehicleResponseDto>>>
    {
        public async Task<Result<List<QueueVehicleResponseDto>>> Handle(GetActiveQueueQuery request, CancellationToken cancellationToken)
        {
            return await repository.GetActiveQueueAsync(request.IdHeadquarter, request.IdRoute, request.IsArrival);
        }
    }
}
