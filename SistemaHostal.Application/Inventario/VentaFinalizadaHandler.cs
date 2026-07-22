using MediatR;
using SistemaHostal.Application.Catalogo;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Inventario;
using SistemaHostal.Domain.Ventas;

namespace SistemaHostal.Application.Inventario;

public class VentaFinalizadaHandler(
    IProductoRepository productoRepository,
    IMovimientoInventarioRepository movimientoRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<VentaFinalizada>
{
    public async Task Handle(VentaFinalizada notification, CancellationToken cancellationToken)
    {
        foreach (var linea in notification.Lineas)
        {
            var producto = await productoRepository.GetByIdAsync(linea.ProductoId, cancellationToken);
            if (producto is null) continue;

            producto.DescontarStock(linea.Cantidad);
            productoRepository.Update(producto);

            var movimiento = new MovimientoInventario(
                linea.ProductoId, TipoMovimiento.Salida, -linea.Cantidad, notification.UsuarioId, motivo: null, ventaId: notification.VentaId);
            await movimientoRepository.AddAsync(movimiento, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}