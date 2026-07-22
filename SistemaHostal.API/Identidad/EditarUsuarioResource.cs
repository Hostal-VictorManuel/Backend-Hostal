using SistemaHostal.Domain.Identidad;

namespace SistemaHostal.API.Resources.Identidad;

public record EditarUsuarioResource(string NombreCompleto, string NombreUsuario, RolUsuario Rol);