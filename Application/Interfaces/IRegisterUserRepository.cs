

using Application.DTOs;

namespace Application.Interfaces
{
    public interface IRegisterUserRepository
    {
        Task<bool> RegisterUser(RegisterUserDto registerUserDto);
    }
}



