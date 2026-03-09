using Application.Common;
using Application.Interfaces.QueueVehicles;
using MediatR;

namespace Application.Features.QueueVehicles.Commands.DispatchVehicle
{
    public record DispatchVehicleCommand(int IdAssignQueue) : IRequest<Result<int>>;

    public class DispatchVehicleCommandHandler : IRequestHandler<DispatchVehicleCommand, Result<int>>
    {
        private readonly IDispatchVehicleRepository _repository;

        public DispatchVehicleCommandHandler(IDispatchVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<int>> Handle(DispatchVehicleCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DispatchVehicleAsync(request.IdAssignQueue);
        }
    }
}
