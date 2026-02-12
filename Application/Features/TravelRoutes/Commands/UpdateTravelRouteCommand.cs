// rutas=darwin
using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.TravelRoutes.Commands
{
    public record UpdateTravelRouteCommand(UpdateTravelRouteDto updateTravelRouteDto) : IRequest<bool>;

    public class UpdateTravelRouteHandler : IRequestHandler<UpdateTravelRouteCommand, bool>
    {
        private readonly IUpdateTravelRouteRepository _repository;
        public UpdateTravelRouteHandler(IUpdateTravelRouteRepository repository) => _repository = repository;

        public async Task<bool> Handle(UpdateTravelRouteCommand request, CancellationToken ct)
        {
            return await _repository.UpdateTravelRoute(request.updateTravelRouteDto);
        }
    }
}
