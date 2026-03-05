using Application.Interfaces.QueueVehicles;
using MediatR;

namespace Application.Features.QueueVehicles.Commands
{
    public record DeleteQueueVehicleCommand(int IdAssignQueue) : IRequest<bool>;

    public class DeleteQueueVehicleHandler : IRequestHandler<DeleteQueueVehicleCommand, bool>
    {
        private readonly IDeleteQueueVehicleRepository _repository;

        public DeleteQueueVehicleHandler(IDeleteQueueVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteQueueVehicleCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteQueueVehicleAsync(request.IdAssignQueue);
        }
    }
}
