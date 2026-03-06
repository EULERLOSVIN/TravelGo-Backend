using Application.Common;
using Application.DTOs.QueueVehicles;
using Application.Interfaces.QueueVehicles;
using MediatR;

namespace Application.Features.QueueVehicles.Queries
{
    public record GetActiveQueueQuery(int IdHeadquarter) : IRequest<Result<List<QueueVehicleResponseDto>>>;

    public class GetActiveQueueHandler : IRequestHandler<GetActiveQueueQuery, Result<List<QueueVehicleResponseDto>>>
    {
        private readonly IGetActiveQueueRepository _repository;

        public GetActiveQueueHandler(IGetActiveQueueRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<QueueVehicleResponseDto>>> Handle(GetActiveQueueQuery request, CancellationToken cancellationToken)
        {
            var queue = await _repository.GetActiveQueueAsync(request.IdHeadquarter);
            return Result<List<QueueVehicleResponseDto>>.Success(queue);
        }
    }
}
