// rutas=darwin Contiene la orden "Obtener Todas las Rutas" y el cocinero (Handler) que sabe usar el repositorio para hacerlo.
using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.TravelRoutes.Queries
{
    public record GetAllTravelRoutesQuery() : IRequest<List<TravelRouteDto>>;

    public class GetAllTravelRoutesHandler : IRequestHandler<GetAllTravelRoutesQuery, List<TravelRouteDto>>
    {
        private readonly IGetAllTravelRoutesRepository _repository;
        public GetAllTravelRoutesHandler(IGetAllTravelRoutesRepository repository) => _repository = repository;

        public async Task<List<TravelRouteDto>> Handle(GetAllTravelRoutesQuery request, CancellationToken ct)
        {
            return await _repository.GetAllTravelRoutes();
        }
    }
}
