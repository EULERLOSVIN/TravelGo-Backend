using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.QueueManagement.Commands.DeleteQueueVehicle;

public class DeleteQueueVehicleCommandHandler : IRequestHandler<DeleteQueueVehicleCommand, bool>
{
    private readonly IQueueManagementRepository _repository;

    public DeleteQueueVehicleCommandHandler(IQueueManagementRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteQueueVehicleCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteQueueVehicle(request.IdQueueVehicle);
    }
}
