

using Application.DTOs;

namespace Application.Interfaces
{
    public interface IRegisterUserRepository
    {
        Task<int> RegisterUser(RegisterUserDto registerUserDto);
    }
}



