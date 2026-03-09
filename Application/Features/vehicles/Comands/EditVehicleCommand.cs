using Application.Common;
using Application.DTOs.vehicles;
using Application.Interfaces;
using MediatR;

namespace Application.Features.vehicles.Comands
{
    public record EditVehicleCommand(EditVehicleDto newData) : IRequest<Result<bool>>;
    public class EditVehicleCommandHandler : IRequestHandler<EditVehicleCommand, Result<bool>>
    {
        private readonly IVehicleRepository  _vehicleRepository;
        public EditVehicleCommandHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<Result<bool>> Handle(EditVehicleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var success = await _vehicleRepository.EditVehicle(request.newData);
                if (!success) {
                    return Result<bool>.Failure("No se pudo editar el vehiculo.");
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Ocurrio un error al editar el vehiculo: " + ex.Message);
            }
        }
    }
}
