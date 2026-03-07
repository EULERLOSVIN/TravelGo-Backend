


using Application.Common;
using Application.DTOs.vehicles;
using Application.DTOs.Vehicles;
using Application.Interfaces;
using MediatR;

namespace Application.Features.vehicles.Queries
{
    public record GetVehiclesByFiltersQuery(FilterVehicleDto Filters) :IRequest<Result<List<DetailVehicleDto>>>;
    public class GetVehiclesByFiltersQueryHandler: IRequestHandler<GetVehiclesByFiltersQuery, Result<List<DetailVehicleDto>>>
    {
        public readonly IVehicleRepository _repository;

        public GetVehiclesByFiltersQueryHandler(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<DetailVehicleDto>>> Handle(GetVehiclesByFiltersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var success = await _repository.GetVehiclesByFilters(request.Filters);
                if (success == null)
                {
                    return Result<List<DetailVehicleDto>>.Failure("No se pudo completar el registro. Es posible que los datos ya existan o sean inválidos.");
                }
                return Result<List<DetailVehicleDto>>.Success(success);
            }
            catch (Exception)
            {
                return Result<List<DetailVehicleDto>>.Failure("Ocurrio un error inesperado al procesar el registro.");
            }
        }
    }
}
