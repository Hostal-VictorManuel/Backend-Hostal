using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Inventario;

public record AjusteRegistrado(int ProductoId, string NombreProducto, int Diferencia, string Motivo, int UsuarioId) : DomainEvent;