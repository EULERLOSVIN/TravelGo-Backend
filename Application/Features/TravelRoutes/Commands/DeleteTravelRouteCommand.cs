// rutas=darwin
using Application.Common;
using Application.Interfaces;
using MediatR;

namespace Application.Features.TravelRoutes.Commands
{
    public record DeleteTravelRouteCommand(int idTravelRoute) : IRequest<Result<bool>>;

    public class DeleteTravelRouteHandler : IRequestHandler<DeleteTravelRouteCommand, Result<bool>>
    {
        private readonly IDeleteTravelRouteRepository _repository;
        public DeleteTravelRouteHandler(IDeleteTravelRouteRepository repository) => _repository = repository;

        public async Task<Result<bool>> Handle(DeleteTravelRouteCommand request, CancellationToken ct)
        {
            var deleted = await _repository.DeleteTravelRoute(request.idTravelRoute);
            return Result<bool>.Success(deleted);
        }
    }
}
