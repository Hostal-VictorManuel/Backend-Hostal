using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Identidad;

public record RestablecerPasswordCommand(int UsuarioId, string NuevaPassword) : IRequest<Result>;