using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Identidad;

public record ActivarDesactivarUsuarioCommand(int UsuarioId, bool Activar) : IRequest<Result>;