// places=darwin
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IGetAllPlacesRepository
    {
        Task<List<PlaceDto>> GetAllPlaces();
    }
}
