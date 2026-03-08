using MediatR;
using Application.DTOs.DepartureTimes;
using Application.Interfaces.DepartureTimes;
using System.Collections.Generic;

namespace Application.Features.DepartureTimes.Queries
{
    public class GetDepartureTimesByRouteQuery : IRequest<List<DepartureTimeDto>>
    {
        public int IdTravelRoute { get; }
        public GetDepartureTimesByRouteQuery(int idTravelRoute) => IdTravelRoute = idTravelRoute;
    }

    public class GetDepartureTimesByRouteQueryHandler : IRequestHandler<GetDepartureTimesByRouteQuery, List<DepartureTimeDto>>
    {
        private readonly IGetDepartureTimesByRouteRepository _repository;

        public GetDepartureTimesByRouteQueryHandler(IGetDepartureTimesByRouteRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DepartureTimeDto>> Handle(GetDepartureTimesByRouteQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetDepartureTimesByRouteAsync(request.IdTravelRoute);
        }
    }
}
