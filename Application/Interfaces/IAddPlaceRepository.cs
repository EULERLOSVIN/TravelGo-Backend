// places=darwin
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IAddPlaceRepository
    {
        Task<int> AddPlace(AddPlaceDto dto);
    }
}
