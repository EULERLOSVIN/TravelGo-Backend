using MediatR;

namespace Application.Features.QueueManagement.Commands.AddQueueVehicle;

public class AddQueueVehicleCommand : IRequest<int>
{
    public int IdVehicle { get; set; }
}
