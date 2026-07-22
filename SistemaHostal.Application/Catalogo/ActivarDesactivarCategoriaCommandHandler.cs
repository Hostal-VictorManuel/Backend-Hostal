using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Catalogo;

namespace SistemaHostal.Application.Catalogo;

public class ActivarDesactivarCategoriaCommandHandler(
    ICategoriaRepository categoriaRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ActivarDesactivarCategoriaCommand, Result>
{
    public async Task<Result> Handle(ActivarDesactivarCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = await categoriaRepository.GetByIdAsync(request.CategoriaId, cancellationToken);
        if (categoria is null)
            return Result.Failure(CatalogoError.CategoriaNoEncontrada, "Categoría no encontrada.");

        if (request.Activar) categoria.Activar();
        else categoria.Desactivar();

        categoriaRepository.Update(categoria);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}