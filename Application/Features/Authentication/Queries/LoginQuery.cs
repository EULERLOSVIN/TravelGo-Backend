using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Authentication.Queries
{
    public record LoginQuery(LoginRequestDto LoginRequestDto) : IRequest<LoginResponseDto?>;

    public class LoginHandler : IRequestHandler<LoginQuery, LoginResponseDto?>
    {
        private readonly ILoginRepository _loginRepository;

        public LoginHandler(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public async Task<LoginResponseDto?> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var result = await _loginRepository.Login(request.LoginRequestDto);
            if (result == null)
            {
                return null;
            }

            return result;
        }
    }
}