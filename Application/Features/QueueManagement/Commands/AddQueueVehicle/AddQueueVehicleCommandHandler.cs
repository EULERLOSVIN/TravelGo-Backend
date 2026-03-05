using MediatR;

namespace Application.Features.QueueManagement.Commands.AddQueueVehicle;

public class AddQueueVehicleCommandHandler : IRequestHandler<AddQueueVehicleCommand, int>
{
    private readonly IQueueManagementRepository _queueRepository;

    public AddQueueVehicleCommandHandler(IQueueManagementRepository queueRepository)
    {
        _queueRepository = queueRepository;
    }

    public async Task<int> Handle(AddQueueVehicleCommand request, CancellationToken cancellationToken)
    {
        return await _queueRepository.AddQueueVehicle(request.IdVehicle);
    }
}
