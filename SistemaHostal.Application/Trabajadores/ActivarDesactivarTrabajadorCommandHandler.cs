using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Trabajadores;

namespace SistemaHostal.Application.Trabajadores;

public class ActivarDesactivarTrabajadorCommandHandler(
    ITrabajadorRepository trabajadorRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ActivarDesactivarTrabajadorCommand, Result>
{
    public async Task<Result> Handle(ActivarDesactivarTrabajadorCommand request, CancellationToken cancellationToken)
    {
        var trabajador = await trabajadorRepository.GetByIdAsync(request.TrabajadorId, cancellationToken);
        if (trabajador is null)
            return Result.Failure(TrabajadoresError.TrabajadorNoEncontrado, "Trabajador no encontrado.");

        if (request.Activar) trabajador.Activar();
        else trabajador.Desactivar();

        trabajadorRepository.Update(trabajador);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}