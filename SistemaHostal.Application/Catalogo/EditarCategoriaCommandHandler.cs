using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Catalogo;

namespace SistemaHostal.Application.Catalogo;

public class EditarCategoriaCommandHandler(
    ICategoriaRepository categoriaRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<EditarCategoriaCommand, Result<CategoriaResumenDto>>
{
    public async Task<Result<CategoriaResumenDto>> Handle(EditarCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = await categoriaRepository.GetByIdAsync(request.CategoriaId, cancellationToken);
        if (categoria is null)
            return Result<CategoriaResumenDto>.Failure(CatalogoError.CategoriaNoEncontrada, "Categoría no encontrada.");

        var nombreEnUso = await categoriaRepository.ExisteNombreAsync(request.Nombre, request.CategoriaId, cancellationToken);
        if (nombreEnUso)
            return Result<CategoriaResumenDto>.Failure(CatalogoError.NombreCategoriaNoDisponible, "Ya existe una categoría con ese nombre.");

        categoria.Editar(request.Nombre);
        categoriaRepository.Update(categoria);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = new CategoriaResumenDto(categoria.Id, categoria.Nombre, categoria.Estado.ToString(), 0, categoria.FechaCreacion);
        return Result<CategoriaResumenDto>.Success(dto);
    }
}