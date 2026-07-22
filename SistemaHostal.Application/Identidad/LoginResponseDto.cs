namespace SistemaHostal.Application.Identidad;

public record LoginResponseDto(
    string Token,
    string NombreCompleto,
    string Rol);