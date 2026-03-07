
using Application.Common;
using Application.DTOs.ManageSales;
using Application.Interfaces.Routes;
using MediatR;

namespace Application.Features.TravelRoutes.Queries
{
    public record GetAllRoutesQuery:IRequest<Result<List<RouteDto>>>;
    public class GetAllRoutesQueryHandler: IRequestHandler<GetAllRoutesQuery, Result<List<RouteDto>>>
    {
        private readonly IRouteRepository _repository;

        public GetAllRoutesQueryHandler(IRouteRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result<List<RouteDto>>> Handle(GetAllRoutesQuery request, CancellationToken ct)
        {
            try
            {
                var success = await _repository.GetAllRoutes();
                if (success == null)
                {
                    return Result<List<RouteDto>>.Failure("Error al obtener las rutas");
                }
                return Result<List<RouteDto>>.Success(success);

            }
            catch (Exception)
            {
                return Result<List<RouteDto>>.Failure("Ocurrio un error al obtener las rutas");
            }
        }
    }
}
