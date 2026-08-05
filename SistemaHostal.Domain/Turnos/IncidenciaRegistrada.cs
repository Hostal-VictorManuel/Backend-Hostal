using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Turnos;

public record IncidenciaRegistrada(int TurnoId, int UsuarioId, string NombreUsuario, string Descripcion) : DomainEvent;