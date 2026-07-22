using MediatR;
using SistemaHostal.Application.Catalogo;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Inventario;

namespace SistemaHostal.Application.Inventario;

public class RegistrarIngresoCommandHandler(
    IProductoRepository productoRepository,
    IMovimientoInventarioRepository movimientoRepository,
    IUnitOfWork unitOfWork,
    IPublisher publisher) : IRequestHandler<RegistrarIngresoCommand, Result<MovimientoInventarioDto>>
{
    public async Task<Result<MovimientoInventarioDto>> Handle(RegistrarIngresoCommand request, CancellationToken cancellationToken)
    {
        var producto = await productoRepository.GetByIdAsync(request.ProductoId, cancellationToken);
        if (producto is null)
            return Result<MovimientoInventarioDto>.Failure(InventarioError.ProductoNoEncontrado, "Producto no encontrado.");

        producto.IncrementarStock(request.Cantidad);
        productoRepository.Update(producto);

        var movimiento = new MovimientoInventario(request.ProductoId, TipoMovimiento.Ingreso, request.Cantidad, request.UsuarioId, null, null);
        await movimientoRepository.AddAsync(movimiento, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await publisher.Publish(new IngresoRegistrado(producto.Id, producto.Nombre, request.Cantidad, request.UsuarioId), cancellationToken);


        var dto = new MovimientoInventarioDto(
            movimiento.Id, movimiento.ProductoId, producto.Nombre, movimiento.Tipo.ToString(),
            movimiento.Cantidad, movimiento.Motivo, movimiento.VentaId, movimiento.UsuarioId, string.Empty, movimiento.FechaHora);

        return Result<MovimientoInventarioDto>.Success(dto);
    }
}