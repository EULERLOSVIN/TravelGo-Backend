using Application.Common;
using Application.DTOs.QueueVehicles;
using Application.Interfaces.QueueVehicles;
using MediatR;

namespace Application.Features.QueueVehicles.Commands
{
    public record UpdateQueueVehicleRouteCommand(UpdateRouteQueueVehicleDto UpdateRouteDto) : IRequest<Result<bool>>;

    public class UpdateQueueVehicleRouteHandler : IRequestHandler<UpdateQueueVehicleRouteCommand, Result<bool>>
    {
        private readonly IUpdateQueueVehicleRouteRepository _repository;

        public UpdateQueueVehicleRouteHandler(IUpdateQueueVehicleRouteRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(UpdateQueueVehicleRouteCommand request, CancellationToken cancellationToken)
        {
            var updated = await _repository.UpdateQueueVehicleRouteAsync(request.UpdateRouteDto);
            return Result<bool>.Success(updated);
        }
    }
}
