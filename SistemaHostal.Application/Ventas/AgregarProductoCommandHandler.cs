using MediatR;
using SistemaHostal.Application.Catalogo;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Ventas;

namespace SistemaHostal.Application.Ventas;

public class AgregarProductoCommandHandler(
    IVentaRepository ventaRepository,
    IProductoRepository productoRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AgregarProductoCommand, Result<VentaDetalleDto>>
{
    public async Task<Result<VentaDetalleDto>> Handle(AgregarProductoCommand request, CancellationToken cancellationToken)
    {
        var venta = await ventaRepository.ObtenerConLineasYPagosAsync(request.VentaId, cancellationToken);
        if (venta is null)
            return Result<VentaDetalleDto>.Failure(VentasError.VentaNoEncontrada, "Venta no encontrada.");

        var producto = await productoRepository.GetByIdAsync(request.ProductoId, cancellationToken);
        if (producto is null)
            return Result<VentaDetalleDto>.Failure(Domain.Catalogo.CatalogoError.ProductoNoEncontrado, "Producto no encontrado.");

        var cantidadYaEnCarrito = venta.LineasVenta.FirstOrDefault(l => l.ProductoId == request.ProductoId)?.Cantidad ?? 0;
        if (!producto.TieneStockSuficiente(cantidadYaEnCarrito + request.Cantidad))
            return Result<VentaDetalleDto>.Failure(VentasError.StockInsuficiente, "No hay stock suficiente para esa cantidad.");

        try
        {
            venta.AgregarProducto(producto.Id, producto.Nombre, producto.Precio, request.Cantidad);
        }
        catch (InvalidOperationException ex)
        {
            return Result<VentaDetalleDto>.Failure(VentasError.VentaNoEstaEnProceso, ex.Message);
        }

        ventaRepository.Update(venta);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<VentaDetalleDto>.Success(MapearDetalle(venta));
    }

    private static VentaDetalleDto MapearDetalle(Domain.Ventas.Venta venta) => new(
        venta.Id, venta.NumeroVenta, venta.TurnoId, venta.NumeroHabitacion, venta.Observaciones,
        venta.Total, venta.VueltoEfectivo, venta.Estado.ToString(), venta.FechaHoraInicio, venta.FechaHoraFinalizacion,
        venta.LineasVenta.Select(l => new LineaVentaDto(l.Id, l.ProductoId, l.NombreProducto, l.PrecioUnitario, l.Cantidad, l.Subtotal)).ToList(),
        venta.PagosVenta.Select(p => new PagoVentaDto(p.Id, p.MetodoDePagoId, p.Monto, p.ReferenciaPago)).ToList());
}