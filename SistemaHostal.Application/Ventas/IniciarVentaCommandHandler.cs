using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Ventas;

namespace SistemaHostal.Application.Ventas;

public class IniciarVentaCommandHandler(
    IVentaRepository ventaRepository,
    INumeroVentaGenerator numeroVentaGenerator,
    IUnitOfWork unitOfWork) : IRequestHandler<IniciarVentaCommand, Result<VentaResumenDto>>
{
    public async Task<Result<VentaResumenDto>> Handle(IniciarVentaCommand request, CancellationToken cancellationToken)
    {
        var numeroVenta = await numeroVentaGenerator.GenerarAsync(cancellationToken);
        var venta = new Venta(numeroVenta, request.TurnoId, request.NumeroHabitacion);

        await ventaRepository.AddAsync(venta, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = new VentaResumenDto(
            venta.Id, venta.NumeroVenta, venta.TurnoId, venta.NumeroHabitacion,
            venta.Total, venta.Estado.ToString(), venta.FechaHoraInicio, venta.FechaHoraFinalizacion);

        return Result<VentaResumenDto>.Success(dto);
    }
}