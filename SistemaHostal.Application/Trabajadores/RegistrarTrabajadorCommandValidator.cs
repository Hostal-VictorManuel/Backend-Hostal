using FluentValidation;

namespace SistemaHostal.Application.Trabajadores;

public class RegistrarTrabajadorCommandValidator : AbstractValidator<RegistrarTrabajadorCommand>
{
    public RegistrarTrabajadorCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(150);
    }
}