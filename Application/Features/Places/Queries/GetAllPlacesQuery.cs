// places=darwin
using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Places.Queries
{
    public record GetAllPlacesQuery : IRequest<List<PlaceDto>>;

    public class GetAllPlacesHandler : IRequestHandler<GetAllPlacesQuery, List<PlaceDto>>
    {
        private readonly IGetAllPlacesRepository _repository;
        public GetAllPlacesHandler(IGetAllPlacesRepository repository) => _repository = repository;

        public async Task<List<PlaceDto>> Handle(GetAllPlacesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllPlaces();
        }
    }
}
