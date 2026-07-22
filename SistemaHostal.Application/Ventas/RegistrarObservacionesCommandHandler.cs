using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Ventas;

namespace SistemaHostal.Application.Ventas;

public class RegistrarObservacionesCommandHandler(
    IVentaRepository ventaRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RegistrarObservacionesCommand, Result>
{
    public async Task<Result> Handle(RegistrarObservacionesCommand request, CancellationToken cancellationToken)
    {
        var venta = await ventaRepository.GetByIdAsync(request.VentaId, cancellationToken);
        if (venta is null)
            return Result.Failure(VentasError.VentaNoEncontrada, "Venta no encontrada.");

        venta.RegistrarObservaciones(request.Observaciones);
        ventaRepository.Update(venta);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}