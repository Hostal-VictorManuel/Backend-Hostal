using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Turnos;

public record RegistrarIncidenciaCommand(int TurnoId, string Descripcion) : IRequest<Result>;