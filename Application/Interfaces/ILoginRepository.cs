using Application.DTOs;

namespace Application.Interfaces
{
    public interface ILoginRepository
    {
        Task<LoginResponseDto?> Login(LoginRequestDto loginRequestDto);
    }
}