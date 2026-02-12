// rutas=darwin
using Application.Interfaces;
using MediatR;

namespace Application.Features.TravelRoutes.Commands
{
    public record DeleteTravelRouteCommand(int idTravelRoute) : IRequest<bool>;

    public class DeleteTravelRouteHandler : IRequestHandler<DeleteTravelRouteCommand, bool>
    {
        private readonly IDeleteTravelRouteRepository _repository;
        public DeleteTravelRouteHandler(IDeleteTravelRouteRepository repository) => _repository = repository;

        public async Task<bool> Handle(DeleteTravelRouteCommand request, CancellationToken ct)
        {
            return await _repository.DeleteTravelRoute(request.idTravelRoute);
        }
    }
}
