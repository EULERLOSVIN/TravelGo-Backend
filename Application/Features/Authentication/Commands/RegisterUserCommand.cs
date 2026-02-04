using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Authentication.Commands
{
    public record RegisterUserCommand(RegisterUserDto RegisterUserDto) : IRequest<int>;

    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, int>
    {
        private readonly IRegisterUserRepository _registerUserRepository;

        public RegisterUserHandler(IRegisterUserRepository registerUserRepository)
        {
            _registerUserRepository = registerUserRepository;
        }

        public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            return await _registerUserRepository.RegisterUser(request.RegisterUserDto);
        }
    }
}

