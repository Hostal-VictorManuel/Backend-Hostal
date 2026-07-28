using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Ventas;

namespace SistemaHostal.Application.Ventas;

public class CerrarMesTrabajadoresCommandHandler(
    IVentaRepository ventaRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CerrarMesTrabajadoresCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CerrarMesTrabajadoresCommand request, CancellationToken cancellationToken)
    {
        var ventasPendientes = await ventaRepository.ObtenerPendientesConTrabajadorAsync(cancellationToken);

        foreach (var venta in ventasPendientes)
        {
            var pagos = new List<(int MetodoDePagoId, decimal Monto, string? ReferenciaPago)>
            {
                (request.MetodoDePagoId, venta.Total, "Cierre de mes - descuento de planilla")
            };

            venta.MarcarComoPagada(pagos);
            ventaRepository.Update(venta);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(ventasPendientes.Count);
    }
}