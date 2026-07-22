using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Turnos;

namespace SistemaHostal.Application.Turnos;

public class RegistrarIncidenciaCommandHandler(
    ITurnoRepository turnoRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RegistrarIncidenciaCommand, Result>
{
    public async Task<Result> Handle(RegistrarIncidenciaCommand request, CancellationToken cancellationToken)
    {
        var turno = await turnoRepository.ObtenerConIncidenciasAsync(request.TurnoId, cancellationToken);
        if (turno is null)
            return Result.Failure(TurnosError.TurnoNoEncontrado, "Turno no encontrado.");

        try
        {
            turno.RegistrarIncidencia(request.Descripcion);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(TurnosError.TurnoYaFinalizado, ex.Message);
        }

        turnoRepository.Update(turno);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}