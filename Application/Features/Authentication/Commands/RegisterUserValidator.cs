using FluentValidation;
using Application.Features.Authentication.Commands;

namespace Application.Features.Authentication.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.RegisterUserDto).NotNull();
            RuleFor(x => x.RegisterUserDto.password)
                .NotEmpty().WithMessage("La contraseña es obligatoria para el registro.");
        }
    }
}