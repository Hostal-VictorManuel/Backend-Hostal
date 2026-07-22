using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Ventas;

namespace SistemaHostal.Application.Ventas;

public class CancelarVentaCommandHandler(
    IVentaRepository ventaRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CancelarVentaCommand, Result>
{
    public async Task<Result> Handle(CancelarVentaCommand request, CancellationToken cancellationToken)
    {
        var venta = await ventaRepository.GetByIdAsync(request.VentaId, cancellationToken);
        if (venta is null)
            return Result.Failure(VentasError.VentaNoEncontrada, "Venta no encontrada.");

        try
        {
            venta.Cancelar();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(VentasError.VentaNoEstaEnProceso, ex.Message);
        }

        ventaRepository.Update(venta);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}