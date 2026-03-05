using Application.Common;
using Application.DTOs.Vehicles;
using Application.Interfaces;
using MediatR;

namespace Application.Features.vehicles.Comands
{
    public record UpdateVehicleCommand(string unitId, CreateVehicleDto dto): IRequest<Result<bool>>;
    public class UpdateVehicleCommandHandler: IRequestHandler<UpdateVehicleCommand, Result<bool>>
    {
        private readonly IVehicleRepository _vehicleRepository;
        public UpdateVehicleCommandHandler(IVehicleRepository repository)
        {
            _vehicleRepository = repository;
        }
        public async Task<Result<bool>> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var success = await _vehicleRepository.UpdateVehicleAsync(request.unitId, request.dto);
                if (!success)
                    return Result<bool>.Failure("No se pudo actualizar el vehículo.");
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                // Loguea el error si tienes un logger
                return Result<bool>.Failure("Ocurrió un error al actualizar el vehículo: " + ex.Message);
            }
        }
    
    }
}
