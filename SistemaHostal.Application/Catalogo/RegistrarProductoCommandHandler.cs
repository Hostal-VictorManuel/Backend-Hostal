using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Catalogo;

namespace SistemaHostal.Application.Catalogo;

public class RegistrarProductoCommandHandler(
    IProductoRepository productoRepository,
    ICategoriaRepository categoriaRepository,
    IUnitOfWork unitOfWork,
    IPublisher publisher) : IRequestHandler<RegistrarProductoCommand, Result<ProductoResumenDto>>
{
    public async Task<Result<ProductoResumenDto>> Handle(RegistrarProductoCommand request, CancellationToken cancellationToken)
    {
        var categoria = await categoriaRepository.GetByIdAsync(request.CategoriaId, cancellationToken);
        if (categoria is null)
            return Result<ProductoResumenDto>.Failure(CatalogoError.CategoriaNoEncontrada, "Categoría no encontrada.");

        var codigoEnUso = await productoRepository.ExisteCodigoBarrasAsync(request.CodigoBarras, cancellationToken: cancellationToken);
        if (codigoEnUso)
            return Result<ProductoResumenDto>.Failure(CatalogoError.CodigoBarrasNoDisponible, "Ya existe un producto con ese código de barras.");

        var producto = new Producto(
            request.CodigoBarras, request.Nombre, request.CategoriaId,
            request.Precio, request.StockInicial, request.StockMinimo, request.ImagenUrl);

        await productoRepository.AddAsync(producto, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await publisher.Publish(new ProductoRegistrado(producto.Id, producto.Nombre, request.UsuarioId), cancellationToken);


        var dto = new ProductoResumenDto(
            producto.Id, producto.CodigoBarras, producto.Nombre, categoria.Nombre,
            producto.Precio, producto.StockActual, producto.StockMinimo, producto.Estado.ToString());

        return Result<ProductoResumenDto>.Success(dto);
    }
}