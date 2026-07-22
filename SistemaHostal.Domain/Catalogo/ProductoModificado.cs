using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Catalogo;

public record ProductoModificado(int ProductoId, string NombreProducto, decimal PrecioAnterior, decimal PrecioNuevo, int UsuarioId) : DomainEvent;