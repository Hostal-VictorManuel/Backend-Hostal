using MediatR;
using SistemaHostal.Application.Catalogo;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Inventario;

namespace SistemaHostal.Application.Inventario;

public class RegistrarAjusteCommandHandler(
    IProductoRepository productoRepository,
    IMovimientoInventarioRepository movimientoRepository,
    IUnitOfWork unitOfWork,
    IPublisher publisher) : IRequestHandler<RegistrarAjusteCommand, Result<MovimientoInventarioDto>>
{
    public async Task<Result<MovimientoInventarioDto>> Handle(RegistrarAjusteCommand request, CancellationToken cancellationToken)
    {
        var producto = await productoRepository.GetByIdAsync(request.ProductoId, cancellationToken);
        if (producto is null)
            return Result<MovimientoInventarioDto>.Failure(InventarioError.ProductoNoEncontrado, "Producto no encontrado.");

        var diferencia = request.NuevoStock - producto.StockActual;
        if (diferencia == 0)
            return Result<MovimientoInventarioDto>.Failure(InventarioError.CantidadInvalida, "El nuevo stock es igual al actual; no hay ajuste que registrar.");

        producto.AjustarStock(request.NuevoStock);
        productoRepository.Update(producto);

        var movimiento = new MovimientoInventario(request.ProductoId, TipoMovimiento.Ajuste, diferencia, request.UsuarioId, request.Motivo, null);
        await movimientoRepository.AddAsync(movimiento, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await publisher.Publish(new AjusteRegistrado(producto.Id, producto.Nombre, diferencia, request.Motivo, request.UsuarioId), cancellationToken);


        var dto = new MovimientoInventarioDto(
            movimiento.Id, movimiento.ProductoId, producto.Nombre, movimiento.Tipo.ToString(),
            movimiento.Cantidad, movimiento.Motivo, movimiento.VentaId, movimiento.UsuarioId, string.Empty, movimiento.FechaHora);

        return Result<MovimientoInventarioDto>.Success(dto);
    }
}