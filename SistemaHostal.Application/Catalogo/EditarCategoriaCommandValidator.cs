using FluentValidation;

namespace SistemaHostal.Application.Catalogo;

public class EditarCategoriaCommandValidator : AbstractValidator<EditarCategoriaCommand>
{
    public EditarCategoriaCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
    }
}