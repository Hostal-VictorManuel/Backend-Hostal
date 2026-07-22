using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Turnos;

namespace SistemaHostal.Application.Turnos;

public class FinalizarTurnoCommandHandler(
    ITurnoRepository turnoRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<FinalizarTurnoCommand, Result>
{
    public async Task<Result> Handle(FinalizarTurnoCommand request, CancellationToken cancellationToken)
    {
        var turno = await turnoRepository.GetByIdAsync(request.TurnoId, cancellationToken);
        if (turno is null)
            return Result.Failure(TurnosError.TurnoNoEncontrado, "Turno no encontrado.");

        try
        {
            turno.Finalizar();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(TurnosError.TurnoYaFinalizado, ex.Message);
        }

        turnoRepository.Update(turno);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // TODO (HU-073): incluir resumen de ventas del turno al finalizar — pendiente hasta que exista el BC de Ventas.
        return Result.Success();
    }
}