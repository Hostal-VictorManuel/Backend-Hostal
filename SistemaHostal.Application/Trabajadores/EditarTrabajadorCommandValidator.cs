using FluentValidation;

namespace SistemaHostal.Application.Trabajadores;

public class EditarTrabajadorCommandValidator : AbstractValidator<EditarTrabajadorCommand>
{
    public EditarTrabajadorCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(150);
    }
}