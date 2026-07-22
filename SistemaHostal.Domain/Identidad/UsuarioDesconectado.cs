using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Identidad;

public record UsuarioDesconectado(int UsuarioId, string NombreCompleto) : DomainEvent;