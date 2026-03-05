using Application.Interfaces;
using MediatR;

namespace Application.Features.Vehicles.Commands
{
    public record DeleteVehicleCommand(string UnitId) : IRequest<bool>; 
    public class DeleteVehicleCommandHandler 
        : IRequestHandler<DeleteVehicleCommand, bool>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public DeleteVehicleCommandHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<bool> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
        {
            return await _vehicleRepository.DeleteVehicleAsync(request.UnitId);
        }
    }
}