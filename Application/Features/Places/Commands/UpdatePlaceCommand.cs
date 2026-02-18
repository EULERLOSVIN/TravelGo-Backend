// places=darwin
using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Places.Commands
{
    public record UpdatePlaceCommand(UpdatePlaceDto dto) : IRequest<bool>;

    public class UpdatePlaceHandler : IRequestHandler<UpdatePlaceCommand, bool>
    {
        private readonly IUpdatePlaceRepository _repository;
        public UpdatePlaceHandler(IUpdatePlaceRepository repository) => _repository = repository;

        public async Task<bool> Handle(UpdatePlaceCommand request, CancellationToken cancellationToken)
        {
            return await _repository.UpdatePlace(request.dto);
        }
    }
}
