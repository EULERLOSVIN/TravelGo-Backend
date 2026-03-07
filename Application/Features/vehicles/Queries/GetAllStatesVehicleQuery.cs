

using Application.Common;
using Application.DTOs.vehicles;
using Application.Interfaces;
using MediatR;

namespace Application.Features.vehicles.Queries
{
    public record GetAllStatesVehicleQuery():IRequest<Result<List<StateVehicleDto>?>>;
    public class GetAllStatesVehicleQueryHandler: IRequestHandler<GetAllStatesVehicleQuery, Result<List<StateVehicleDto>?>>
    {
        private readonly IVehicleRepository _repositoryVehicle;
        public GetAllStatesVehicleQueryHandler(IVehicleRepository repositoryVehicle)
        {
            _repositoryVehicle = repositoryVehicle;
        }
        public async Task<Result<List<StateVehicleDto>?>> Handle(GetAllStatesVehicleQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var success = await _repositoryVehicle.GetStateVehicle();
                if (success == null)
                {
                    return Result<List<StateVehicleDto>?>.Failure("No se pudo completar el registro. Es posible que los datos ya existan o sean inválidos.");
                }
                return Result<List<StateVehicleDto>?>.Success(success);
            }
            catch (Exception)
            {
                return Result<List<StateVehicleDto>?>.Failure("Ocurrio un error inesperado al procesar el registro.");
            }

        }
    }
}
