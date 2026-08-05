using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Identidad;
using SistemaHostal.Domain.Turnos;
using SistemaHostal.Application.Identidad;

namespace SistemaHostal.Application.Turnos;

public class RegistrarIncidenciaCommandHandler(
    ITurnoRepository turnoRepository,
    IUsuarioRepository usuarioRepository,
    IUnitOfWork unitOfWork,
    IPublisher publisher) : IRequestHandler<RegistrarIncidenciaCommand, Result>
{
    public async Task<Result> Handle(RegistrarIncidenciaCommand request, CancellationToken cancellationToken)
    {
        var turno = await turnoRepository.ObtenerConIncidenciasAsync(request.TurnoId, cancellationToken);
        if (turno is null)
            return Result.Failure(TurnosError.TurnoNoEncontrado, "Turno no encontrado.");

        var usuario = await usuarioRepository.GetByIdAsync(turno.UsuarioId, cancellationToken);
        var nombreUsuario = usuario?.NombreCompleto ?? string.Empty;

        try
        {
            turno.RegistrarIncidencia(request.Descripcion, nombreUsuario);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(TurnosError.TurnoYaFinalizado, ex.Message);
        }

        turnoRepository.Update(turno);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in turno.DomainEvents)
            await publisher.Publish(domainEvent, cancellationToken);

        turno.ClearDomainEvents();

        return Result.Success();
    }
}