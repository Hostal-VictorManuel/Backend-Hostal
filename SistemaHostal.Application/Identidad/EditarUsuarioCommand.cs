using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Identidad;

namespace SistemaHostal.Application.Identidad;

public record EditarUsuarioCommand(
    int UsuarioId,
    string NombreCompleto,
    string NombreUsuario,
    RolUsuario Rol) : IRequest<Result<UsuarioResumenDto>>;