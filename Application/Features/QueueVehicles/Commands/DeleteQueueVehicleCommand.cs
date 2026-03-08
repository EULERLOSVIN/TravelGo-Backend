using Application.Common;
using Application.Interfaces.QueueVehicles;
using MediatR;

namespace Application.Features.QueueVehicles.Commands
{
    public record DeleteQueueVehicleCommand(int IdAssignQueue) : IRequest<Result<bool>>;

    public class DeleteQueueVehicleHandler : IRequestHandler<DeleteQueueVehicleCommand, Result<bool>>
    {
        private readonly IDeleteQueueVehicleRepository _repository;

        public DeleteQueueVehicleHandler(IDeleteQueueVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(DeleteQueueVehicleCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _repository.DeleteQueueVehicleAsync(request.IdAssignQueue);
            return Result<bool>.Success(deleted);
        }
    }
}
