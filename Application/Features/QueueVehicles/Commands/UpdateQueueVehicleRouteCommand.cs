using Application.DTOs.QueueVehicles;
using Application.Interfaces.QueueVehicles;
using MediatR;

namespace Application.Features.QueueVehicles.Commands
{
    public record UpdateQueueVehicleRouteCommand(UpdateRouteQueueVehicleDto UpdateRouteDto) : IRequest<bool>;

    public class UpdateQueueVehicleRouteHandler : IRequestHandler<UpdateQueueVehicleRouteCommand, bool>
    {
        private readonly IUpdateQueueVehicleRouteRepository _repository;

        public UpdateQueueVehicleRouteHandler(IUpdateQueueVehicleRouteRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateQueueVehicleRouteCommand request, CancellationToken cancellationToken)
        {
            return await _repository.UpdateQueueVehicleRouteAsync(request.UpdateRouteDto);
        }
    }
}
