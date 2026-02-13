// places=darwin
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IUpdatePlaceRepository
    {
        Task<bool> UpdatePlace(UpdatePlaceDto dto);
    }
}
