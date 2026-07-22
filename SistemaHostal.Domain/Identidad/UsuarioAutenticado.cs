using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Identidad;

public record UsuarioAutenticado(int UsuarioId, string NombreCompleto) : DomainEvent;