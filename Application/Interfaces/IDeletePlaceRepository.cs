// places=darwin
namespace Application.Interfaces
{
    public interface IDeletePlaceRepository
    {
        Task<bool> DeletePlace(int idPlace);
    }
}
