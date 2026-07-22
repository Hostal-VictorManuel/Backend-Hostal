using FluentValidation;

namespace SistemaHostal.Application.Identidad;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.NombreUsuario).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}