// rutas=darwin
using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.TravelRoutes.Commands
{
    public record UpdateTravelRouteCommand(UpdateTravelRouteDto updateTravelRouteDto) : IRequest<Result<bool>>;

    public class UpdateTravelRouteHandler : IRequestHandler<UpdateTravelRouteCommand, Result<bool>>
    {
        private readonly IUpdateTravelRouteRepository _repository;
        public UpdateTravelRouteHandler(IUpdateTravelRouteRepository repository) => _repository = repository;

        public async Task<Result<bool>> Handle(UpdateTravelRouteCommand request, CancellationToken ct)
        {
            var updated = await _repository.UpdateTravelRoute(request.updateTravelRouteDto);
            return Result<bool>.Success(updated);
        }
    }
}
