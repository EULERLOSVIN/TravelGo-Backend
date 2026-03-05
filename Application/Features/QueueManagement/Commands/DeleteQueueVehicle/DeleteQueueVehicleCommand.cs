using MediatR;

namespace Application.Features.QueueManagement.Commands.DeleteQueueVehicle;

public record DeleteQueueVehicleCommand(int IdQueueVehicle) : IRequest<bool>;
