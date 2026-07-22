using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Catalogo;

namespace SistemaHostal.Application.Catalogo;

public class EditarProductoCommandHandler(
    IProductoRepository productoRepository,
    ICategoriaRepository categoriaRepository,
    IUnitOfWork unitOfWork,
    IPublisher publisher) : IRequestHandler<EditarProductoCommand, Result<ProductoResumenDto>>
{
    public async Task<Result<ProductoResumenDto>> Handle(EditarProductoCommand request, CancellationToken cancellationToken)
    {
        var producto = await productoRepository.GetByIdAsync(request.ProductoId, cancellationToken);
        if (producto is null)
            return Result<ProductoResumenDto>.Failure(CatalogoError.ProductoNoEncontrado, "Producto no encontrado.");

        var categoria = await categoriaRepository.GetByIdAsync(request.CategoriaId, cancellationToken);
        if (categoria is null)
            return Result<ProductoResumenDto>.Failure(CatalogoError.CategoriaNoEncontrada, "Categoría no encontrada.");

        var precioAnterior = producto.Precio;

        producto.Editar(request.Nombre, request.CategoriaId, request.Precio, request.StockMinimo, request.ImagenUrl);
        productoRepository.Update(producto);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await publisher.Publish(new ProductoModificado(producto.Id, producto.Nombre, precioAnterior, producto.Precio, request.UsuarioId), cancellationToken);

        var dto = new ProductoResumenDto(
            producto.Id, producto.CodigoBarras, producto.Nombre, categoria.Nombre,
            producto.Precio, producto.StockActual, producto.StockMinimo, producto.Estado.ToString());

        return Result<ProductoResumenDto>.Success(dto);
    }
}