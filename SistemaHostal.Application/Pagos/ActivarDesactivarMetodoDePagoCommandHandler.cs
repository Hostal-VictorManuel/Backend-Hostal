using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Pagos;

namespace SistemaHostal.Application.Pagos;

public class ActivarDesactivarMetodoDePagoCommandHandler(
    IMetodoDePagoRepository metodoDePagoRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ActivarDesactivarMetodoDePagoCommand, Result>
{
    public async Task<Result> Handle(ActivarDesactivarMetodoDePagoCommand request, CancellationToken cancellationToken)
    {
        var metodo = await metodoDePagoRepository.GetByIdAsync(request.MetodoDePagoId, cancellationToken);
        if (metodo is null)
            return Result.Failure(PagosError.MetodoDePagoNoEncontrado, "Método de pago no encontrado.");

        if (request.Activar) metodo.Activar();
        else metodo.Desactivar();

        metodoDePagoRepository.Update(metodo);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}