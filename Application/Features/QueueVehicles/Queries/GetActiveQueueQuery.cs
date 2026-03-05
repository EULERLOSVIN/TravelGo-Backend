using Application.DTOs.QueueVehicles;
using Application.Interfaces.QueueVehicles;
using MediatR;

namespace Application.Features.QueueVehicles.Queries
{
    public record GetActiveQueueQuery(int IdHeadquarter) : IRequest<List<QueueVehicleResponseDto>>;

    public class GetActiveQueueHandler : IRequestHandler<GetActiveQueueQuery, List<QueueVehicleResponseDto>>
    {
        private readonly IGetActiveQueueRepository _repository;

        public GetActiveQueueHandler(IGetActiveQueueRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<QueueVehicleResponseDto>> Handle(GetActiveQueueQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetActiveQueueAsync(request.IdHeadquarter);
        }
    }
}
