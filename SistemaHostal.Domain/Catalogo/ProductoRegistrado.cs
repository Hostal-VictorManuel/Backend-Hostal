using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Catalogo;

public record ProductoRegistrado(int ProductoId, string NombreProducto, int UsuarioId) : DomainEvent;