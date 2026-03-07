using Application.Common;
using Application.DTOs.Vehicles;
using Application.Interfaces;
using MediatR;

namespace Application.Features.vehicles.Comands
{
    public record RegisterVehicleCommand(CreateVehicleDto dto): IRequest<Result<bool>>;
    public class RegisterVehicleCommandHandler: IRequestHandler<RegisterVehicleCommand, Result<bool>>
    {
        private IVehicleRepository _vehicleRepository;
        public RegisterVehicleCommandHandler(IVehicleRepository repository)
        {
            _vehicleRepository = repository;
        }
        public async Task<Result<bool>> Handle(RegisterVehicleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var success = await _vehicleRepository.RegisterVehicle(request.dto);
                if (!success) {
                    return Result<bool>.Failure("No se pudo crear el vehiculo.");
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Ocurrio un error al crear el vehiculo: " + ex.Message);
            }
        }
    }
}
