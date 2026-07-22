using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Inventario;

public record IngresoRegistrado(int ProductoId, string NombreProducto, int Cantidad, int UsuarioId) : DomainEvent;