using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Catalogo;

namespace SistemaHostal.Application.Catalogo;

public class ActivarDesactivarProductoCommandHandler(
    IProductoRepository productoRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ActivarDesactivarProductoCommand, Result>
{
    public async Task<Result> Handle(ActivarDesactivarProductoCommand request, CancellationToken cancellationToken)
    {
        var producto = await productoRepository.GetByIdAsync(request.ProductoId, cancellationToken);
        if (producto is null)
            return Result.Failure(CatalogoError.ProductoNoEncontrado, "Producto no encontrado.");

        if (request.Activar) producto.Activar();
        else producto.Desactivar();

        productoRepository.Update(producto);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}