using FluentValidation;

namespace SistemaHostal.Application.Catalogo;

public class RegistrarCategoriaCommandValidator : AbstractValidator<RegistrarCategoriaCommand>
{
    public RegistrarCategoriaCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
    }
}