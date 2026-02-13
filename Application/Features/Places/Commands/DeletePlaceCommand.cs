// places=darwin
using Application.Interfaces;
using MediatR;

namespace Application.Features.Places.Commands
{
    public record DeletePlaceCommand(int idPlace) : IRequest<bool>;

    public class DeletePlaceHandler : IRequestHandler<DeletePlaceCommand, bool>
    {
        private readonly IDeletePlaceRepository _repository;
        public DeletePlaceHandler(IDeletePlaceRepository repository) => _repository = repository;

        public async Task<bool> Handle(DeletePlaceCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeletePlace(request.idPlace);
        }
    }
}
