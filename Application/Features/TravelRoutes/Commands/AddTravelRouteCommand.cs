// rutas=darwin Contiene la orden "Agregar Ruta" y el cocinero (Handler) que sabe usar el repositorio para hacerlo.
using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.TravelRoutes.Commands
{
    // El Pedido
    public record AddTravelRouteCommand(AddTravelRouteDto addTravelRouteDto) : IRequest<int>;

    // El Cocinero
    public class AddTravelRouteHandler : IRequestHandler<AddTravelRouteCommand, int>
    {
        private readonly IAddTravelRouteRepository _repository;
        public AddTravelRouteHandler(IAddTravelRouteRepository repository) => _repository = repository;

        public async Task<int> Handle(AddTravelRouteCommand request, CancellationToken ct)
        {
            return await _repository.AddTravelRoute(request.addTravelRouteDto);
        }
    }
}
