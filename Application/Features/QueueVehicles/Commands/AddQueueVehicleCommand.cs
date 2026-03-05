using Application.DTOs.QueueVehicles;
using Application.Interfaces.QueueVehicles;
using MediatR;

namespace Application.Features.QueueVehicles.Commands
{
    public record AddQueueVehicleCommand(AddQueueVehicleDto AddQueueVehicleDto) : IRequest<int>;

    public class AddQueueVehicleHandler : IRequestHandler<AddQueueVehicleCommand, int>
    {
        private readonly IAddQueueVehicleRepository _repository;

        public AddQueueVehicleHandler(IAddQueueVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(AddQueueVehicleCommand request, CancellationToken cancellationToken)
        {
            return await _repository.AddQueueVehicleAsync(request.AddQueueVehicleDto);
        }
    }
}
