using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Ventas;

public class MarcarTrabajadorComoPagadoCommandHandler(
    IVentaRepository ventaRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<MarcarTrabajadorComoPagadoCommand, Result<int>>
{
    public async Task<Result<int>> Handle(MarcarTrabajadorComoPagadoCommand request, CancellationToken cancellationToken)
    {
        var ventas = await ventaRepository.ObtenerPendientesPorTrabajadorAsync(request.TrabajadorId, cancellationToken);

        foreach (var venta in ventas)
        {
            var pagos = new List<(int MetodoDePagoId, decimal Monto, string? ReferenciaPago)>
            {
                (request.MetodoDePagoId, venta.Total, "Pago de deuda de trabajador")
            };

            venta.MarcarComoPagada(pagos);
            ventaRepository.Update(venta);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(ventas.Count);
    }
}