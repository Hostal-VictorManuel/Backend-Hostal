using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Identidad;

namespace SistemaHostal.Application.Identidad;

public record RegistrarUsuarioCommand(
    string NombreCompleto,
    string NombreUsuario,
    string Password,
    RolUsuario Rol) : IRequest<Result<UsuarioResumenDto>>;