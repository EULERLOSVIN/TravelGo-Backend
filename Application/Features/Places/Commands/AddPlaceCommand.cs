// places=darwin
using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Places.Commands
{
    public record AddPlaceCommand(AddPlaceDto dto) : IRequest<int>;

    public class AddPlaceHandler : IRequestHandler<AddPlaceCommand, int>
    {
        private readonly IAddPlaceRepository _repository;
        public AddPlaceHandler(IAddPlaceRepository repository) => _repository = repository;

        public async Task<int> Handle(AddPlaceCommand request, CancellationToken cancellationToken)
        {
            return await _repository.AddPlace(request.dto);
        }
    }
}
