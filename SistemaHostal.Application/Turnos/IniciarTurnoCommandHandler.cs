using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Turnos;

namespace SistemaHostal.Application.Turnos;

public class IniciarTurnoCommandHandler(
    ITurnoRepository turnoRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<IniciarTurnoCommand, Result<TurnoResumenDto>>
{
    public async Task<Result<TurnoResumenDto>> Handle(IniciarTurnoCommand request, CancellationToken cancellationToken)
    {
        var turnoActivo = await turnoRepository.ObtenerTurnoActivoAsync(request.UsuarioId, cancellationToken);
        if (turnoActivo is not null)
            return Result<TurnoResumenDto>.Failure(TurnosError.UsuarioYaTieneTurnoActivo, "Ya tienes un turno activo.");

        var turno = new Turno(request.UsuarioId);
        await turnoRepository.AddAsync(turno, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = new TurnoResumenDto(turno.Id, turno.UsuarioId, string.Empty, turno.FechaHoraInicio, turno.FechaHoraFin, turno.Estado.ToString());
        return Result<TurnoResumenDto>.Success(dto);
    }
}