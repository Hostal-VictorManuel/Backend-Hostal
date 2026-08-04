using MediatR;
using SistemaHostal.Application.Catalogo;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Inventario;
using SistemaHostal.Domain.Ventas;

namespace SistemaHostal.Application.Inventario;

public class VentaAnuladaHandler(
    IProductoRepository productoRepository,
    IMovimientoInventarioRepository movimientoRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<VentaAnulada>
{
    public async Task Handle(VentaAnulada notification, CancellationToken cancellationToken)
    {
        foreach (var linea in notification.Lineas)
        {
            var producto = await productoRepository.GetByIdAsync(linea.ProductoId, cancellationToken);
            if (producto is null) continue;

            producto.IncrementarStock(linea.Cantidad);
            productoRepository.Update(producto);

            var movimiento = new MovimientoInventario(
                linea.ProductoId, TipoMovimiento.Anulacion, linea.Cantidad, notification.UsuarioId,
                motivo: notification.Motivo, ventaId: notification.VentaId);
            await movimientoRepository.AddAsync(movimiento, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}