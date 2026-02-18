using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Authentication.Queries
{
    public record LoginQuery(LoginRequestDto LoginRequestDto) : IRequest<Result<LoginResponseDto>>;

    public class LoginHandler : IRequestHandler<LoginQuery, Result<LoginResponseDto>>
    {
        private readonly ILoginRepository _loginRepository;

        public LoginHandler(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public async Task<Result<LoginResponseDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var result = await _loginRepository.Login(request.LoginRequestDto);

            if (result == null)
            {
                return Result<LoginResponseDto>.Failure("Credenciales incorrectas o la cuenta no está activa.");
            }

            return Result<LoginResponseDto>.Success(result);
        }
    }
}