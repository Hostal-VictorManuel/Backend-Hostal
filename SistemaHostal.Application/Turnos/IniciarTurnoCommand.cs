using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Turnos;

public record IniciarTurnoCommand(int UsuarioId) : IRequest<Result<TurnoResumenDto>>;
