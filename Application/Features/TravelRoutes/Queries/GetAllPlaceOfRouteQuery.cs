


using Application.Common;
using Application.DTOs.Customers;
using Application.Interfaces.Customers;
using MediatR;

namespace Application.Features.TravelRoutes.Queries
{
    public record GetAllPlaceOfRouteQuery() : IRequest<Result<List<GetPlaceDto>>>;
    public class GetAllPlaceOfRouteQueryHandler: IRequestHandler<GetAllPlaceOfRouteQuery, Result<List<GetPlaceDto>>>
    {
        private readonly IGetAllPlaceofRouteRepository _repository;
        public GetAllPlaceOfRouteQueryHandler(IGetAllPlaceofRouteRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result<List<GetPlaceDto>>> Handle(GetAllPlaceOfRouteQuery request, CancellationToken ct)
        {
            try
            {
                var success = await _repository.GetAllPlaceofRoute();
                if (success == null)
                {
                    return Result<List<GetPlaceDto>>.Failure("Error al obtener los lugares");
                }
                return Result<List<GetPlaceDto>>.Success(success);
            }
            catch (Exception)
            {

                return Result<List<GetPlaceDto>>.Failure("Ocurrió una excepción al obtener los lugares.");
            }
        }
    }
}
