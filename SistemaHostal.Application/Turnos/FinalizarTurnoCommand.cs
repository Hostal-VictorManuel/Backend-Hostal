using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Turnos;

public record FinalizarTurnoCommand(int TurnoId) : IRequest<Result>;