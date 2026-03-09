using Application.DTOs;
using Application.Interfaces.QueueVehicles;
using MediatR;

namespace Application.Features.TravelRoutes.Queries
{
    public record GetRoutesByHeadquarterQuery(int IdHeadquarter, string Type) : IRequest<List<TravelRouteDto>>;

    public class GetRoutesByHeadquarterQueryHandler : IRequestHandler<GetRoutesByHeadquarterQuery, List<TravelRouteDto>>
    {
        private readonly IGetRoutesByHeadquarterRepository _repository;

        public GetRoutesByHeadquarterQueryHandler(IGetRoutesByHeadquarterRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TravelRouteDto>> Handle(GetRoutesByHeadquarterQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetRoutesByHeadquarterAsync(request.IdHeadquarter, request.Type);
        }
    }
}
