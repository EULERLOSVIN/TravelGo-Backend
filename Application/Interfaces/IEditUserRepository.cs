using Application.DTOs;

namespace Application.Interfaces
{
    public interface IEditUserRepository
    {
        Task<bool> EditUser(EditPersonnelDto newData);
    }
}
