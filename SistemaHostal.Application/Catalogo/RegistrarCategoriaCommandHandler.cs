using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Catalogo;

namespace SistemaHostal.Application.Catalogo;

public class RegistrarCategoriaCommandHandler(
    ICategoriaRepository categoriaRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RegistrarCategoriaCommand, Result<CategoriaResumenDto>>
{
    public async Task<Result<CategoriaResumenDto>> Handle(RegistrarCategoriaCommand request, CancellationToken cancellationToken)
    {
        var existe = await categoriaRepository.ExisteNombreAsync(request.Nombre, cancellationToken: cancellationToken);
        if (existe)
            return Result<CategoriaResumenDto>.Failure(CatalogoError.NombreCategoriaNoDisponible, "Ya existe una categoría con ese nombre.");

        var categoria = new Categoria(request.Nombre);
        await categoriaRepository.AddAsync(categoria, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = new CategoriaResumenDto(categoria.Id, categoria.Nombre, categoria.Estado.ToString(), 0, categoria.FechaCreacion);
        return Result<CategoriaResumenDto>.Success(dto);
    }
}