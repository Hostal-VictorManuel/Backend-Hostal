using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Ventas;

namespace SistemaHostal.Application.Ventas;

public class EliminarLineaCommandHandler(
    IVentaRepository ventaRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<EliminarLineaCommand, Result<VentaDetalleDto>>
{
    public async Task<Result<VentaDetalleDto>> Handle(EliminarLineaCommand request, CancellationToken cancellationToken)
    {
        var venta = await ventaRepository.ObtenerConLineasYPagosAsync(request.VentaId, cancellationToken);
        if (venta is null)
            return Result<VentaDetalleDto>.Failure(VentasError.VentaNoEncontrada, "Venta no encontrada.");

        try
        {
            venta.EliminarLinea(request.LineaVentaId);
        }
        catch (InvalidOperationException ex)
        {
            return Result<VentaDetalleDto>.Failure(VentasError.LineaNoEncontrada, ex.Message);
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