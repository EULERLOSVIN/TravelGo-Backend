// rutas=darwin
using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.TravelRoutes.Queries
{
    public record GetAllTravelRoutesQuery() : IRequest<Result<List<TravelRouteDto>>>;

    public class GetAllTravelRoutesHandler : IRequestHandler<GetAllTravelRoutesQuery, Result<List<TravelRouteDto>>>
    {
        private readonly IGetAllTravelRoutesRepository _repository;
        public GetAllTravelRoutesHandler(IGetAllTravelRoutesRepository repository) => _repository = repository;

        public async Task<Result<List<TravelRouteDto>>> Handle(GetAllTravelRoutesQuery request, CancellationToken ct)
        {
            var routes = await _repository.GetAllTravelRoutes();
            return Result<List<TravelRouteDto>>.Success(routes);
        }
    }
}
