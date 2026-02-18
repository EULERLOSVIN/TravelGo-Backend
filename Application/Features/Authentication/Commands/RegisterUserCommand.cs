using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Authentication.Commands
{
    public record RegisterUserCommand(RegisterUserDto RegisterUserDto) : IRequest<Result<bool>>;

    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<bool>>
    {
        private readonly IRegisterUserRepository _registerUserRepository;

        public RegisterUserHandler(IRegisterUserRepository registerUserRepository)
        {
            _registerUserRepository = registerUserRepository;
        }

        public async Task<Result<bool>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                bool success = await _registerUserRepository.RegisterUser(request.RegisterUserDto);

                if (!success)
                {
                    return Result<bool>.Failure("No se pudo completar el registro. Es posible que los datos ya existan o sean inválidos.");
                }
                return Result<bool>.Success(true);
            }
            catch (Exception)
            {
                return Result<bool>.Failure("Ocurrió un error inesperado al procesar el registro.");
            }
        }
    }
}

