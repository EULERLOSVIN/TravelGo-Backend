using Application.Common;
using Application.DTOs.vehicles;
using Application.Interfaces;
using MediatR;

namespace Application.Features.vehicles.Queries
{
    public record GetStatisticalSummaryOfVehiclesQuery : IRequest<Result<SummaryStatisticalOfVehicleDto>>;
    public class GetStatisticalSummaryOfVehiclesQueryHandelr:IRequestHandler<GetStatisticalSummaryOfVehiclesQuery,Result<SummaryStatisticalOfVehicleDto>>
    {
        private readonly IVehicleRepository _repository;

        public GetStatisticalSummaryOfVehiclesQueryHandelr(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<SummaryStatisticalOfVehicleDto>> Handle(GetStatisticalSummaryOfVehiclesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var success = await _repository.GetStatisticalSummaryOfVehicles();
                if (success == null)
                {
                    return Result<SummaryStatisticalOfVehicleDto>.Failure("No se pudo completar el registro. Es posible que los datos ya existan o sean inválidos.");
                }
                return Result<SummaryStatisticalOfVehicleDto>.Success(success);
            }
            catch (Exception)
            {
                return Result<SummaryStatisticalOfVehicleDto>.Failure("Ocurrio un error inesperado al procesar el registro.");
            }
        }
    }
}
