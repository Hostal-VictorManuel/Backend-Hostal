using MediatR;
using SistemaHostal.Application.Catalogo;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Ventas;


namespace SistemaHostal.Application.Ventas;

public class FinalizarVentaCommandHandler(
    IVentaRepository ventaRepository,
    IProductoRepository productoRepository,
    IUnitOfWork unitOfWork,
    IPublisher publisher) : IRequestHandler<FinalizarVentaCommand, Result<VentaDetalleDto>>
{
    public async Task<Result<VentaDetalleDto>> Handle(FinalizarVentaCommand request, CancellationToken cancellationToken)
    {
        var venta = await ventaRepository.ObtenerConLineasYPagosAsync(request.VentaId, cancellationToken);
        if (venta is null)
            return Result<VentaDetalleDto>.Failure(VentasError.VentaNoEncontrada, "Venta no encontrada.");

        if (venta.LineasVenta.Count == 0)
            return Result<VentaDetalleDto>.Failure(VentasError.VentaSinLineas, "No se puede finalizar una venta sin productos.");

        foreach (var linea in venta.LineasVenta)
        {
            var producto = await productoRepository.GetByIdAsync(linea.ProductoId, cancellationToken);
            if (producto is null || !producto.TieneStockSuficiente(linea.Cantidad))
                return Result<VentaDetalleDto>.Failure(
                    VentasError.StockInsuficiente,
                    $"No hay stock suficiente para \"{linea.NombreProducto}\".");
        }

        if (!request.CargarAHabitacion)
        {
            var totalPagado = request.Pagos.Sum(p => p.Monto);
            if (totalPagado < venta.Total)
                return Result<VentaDetalleDto>.Failure(VentasError.MontoDePagoNoCoincideConTotal, "La suma de los pagos no cubre el total de la venta.");
        }

        try
        {
            var pagos = request.Pagos.Select(p => (p.MetodoDePagoId, p.Monto, p.ReferenciaPago)).ToList();
            venta.Finalizar(pagos, request.CargarAHabitacion, request.UsuarioId);
        }
        catch (InvalidOperationException ex)
        {
            return Result<VentaDetalleDto>.Failure(VentasError.VentaNoEstaEnProceso, ex.Message);
        }

        ventaRepository.Update(venta);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in venta.DomainEvents)
            await publisher.Publish(domainEvent, cancellationToken);

        venta.ClearDomainEvents();

        return Result<VentaDetalleDto>.Success(MapearDetalle(venta));
    }

    private static VentaDetalleDto MapearDetalle(Domain.Ventas.Venta venta) => new(
        venta.Id, venta.NumeroVenta, venta.TurnoId, venta.NumeroHabitacion, venta.Observaciones,
        venta.Total, venta.VueltoEfectivo, venta.Estado.ToString(), venta.FechaHoraInicio, venta.FechaHoraFinalizacion,
        venta.LineasVenta.Select(l => new LineaVentaDto(l.Id, l.ProductoId, l.NombreProducto, l.PrecioUnitario, l.Cantidad, l.Subtotal)).ToList(),
        venta.PagosVenta.Select(p => new PagoVentaDto(p.Id, p.MetodoDePagoId, p.Monto, p.ReferenciaPago)).ToList());
}