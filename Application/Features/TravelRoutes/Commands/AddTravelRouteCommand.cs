// rutas=darwin
using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.TravelRoutes.Commands
{
    public record AddTravelRouteCommand(AddTravelRouteDto addTravelRouteDto) : IRequest<Result<int>>;

    public class AddTravelRouteHandler : IRequestHandler<AddTravelRouteCommand, Result<int>>
    {
        private readonly IAddTravelRouteRepository _repository;
        public AddTravelRouteHandler(IAddTravelRouteRepository repository) => _repository = repository;

        public async Task<Result<int>> Handle(AddTravelRouteCommand request, CancellationToken ct)
        {
            var id = await _repository.AddTravelRoute(request.addTravelRouteDto);
            return Result<int>.Success(id);
        }
    }
}
