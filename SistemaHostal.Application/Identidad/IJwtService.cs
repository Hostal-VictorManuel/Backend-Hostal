using SistemaHostal.Domain.Identidad;

namespace SistemaHostal.Application.Identidad;

public interface IJwtService
{
    string GenerarToken(int usuarioId, string nombreCompleto, RolUsuario rol);
}