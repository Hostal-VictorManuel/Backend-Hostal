using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Application.Identidad;
using SistemaHostal.Domain.Ventas;

namespace SistemaHostal.Application.Ventas;

public class TransferirHabitacionCommandHandler(
    IVentaRepository ventaRepository,
    IUsuarioRepository usuarioRepository,
    IUnitOfWork unitOfWork,
    IPublisher publisher) : IRequestHandler<TransferirHabitacionCommand, Result<VentaDetalleDto>>
{
    public async Task<Result<VentaDetalleDto>> Handle(TransferirHabitacionCommand request, CancellationToken cancellationToken)
    {
        var venta = await ventaRepository.ObtenerConLineasYPagosAsync(request.VentaId, cancellationToken);
        if (venta is null)
            return Result<VentaDetalleDto>.Failure(VentasError.VentaNoEncontrada, "Venta no encontrada.");

        try
        {
            venta.TransferirHabitacion(request.NumeroHabitacionNueva, request.Motivo, request.UsuarioId);
        }
        catch (InvalidOperationException ex)
        {
            return Result<VentaDetalleDto>.Failure(VentasError.VentaNoSePuedeTransferir, ex.Message);
        }

        ventaRepository.Update(venta);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in venta.DomainEvents)
            await publisher.Publish(domainEvent, cancellationToken);
        venta.ClearDomainEvents();

        var usuario = await usuarioRepository.GetByIdAsync(request.UsuarioId, cancellationToken);
        var dto = MapearDetalle(venta, usuario?.NombreCompleto ?? string.Empty);

        return Result<VentaDetalleDto>.Success(dto);
    }

    private static VentaDetalleDto MapearDetalle(Domain.Ventas.Venta venta, string nombreUsuarioTransferencia) => new(
        venta.Id, venta.NumeroVenta, venta.TurnoId, venta.NumeroHabitacion, venta.TrabajadorId, venta.Observaciones,
        venta.Total, venta.VueltoEfectivo, venta.Estado.ToString(),
        venta.MotivoAnulacion, venta.UsuarioAnulacionId, string.Empty, venta.FechaHoraAnulacion,
        venta.HabitacionAnterior, venta.MotivoTransferencia, venta.UsuarioTransferenciaId, nombreUsuarioTransferencia, venta.FechaHoraTransferencia,
        venta.FechaHoraInicio, venta.FechaHoraFinalizacion,
        venta.LineasVenta.Select(l => new LineaVentaDto(l.Id, l.ProductoId, l.NombreProducto, l.PrecioUnitario, l.Cantidad, l.Subtotal)).ToList(),
        venta.PagosVenta.Select(p => new PagoVentaDto(p.Id, p.MetodoDePagoId, p.Monto, p.ReferenciaPago)).ToList());
}