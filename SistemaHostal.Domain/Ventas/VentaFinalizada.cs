using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Ventas;

public record LineaVentaFinalizada(int ProductoId, int Cantidad);

public record VentaFinalizada(int VentaId, int UsuarioId, IReadOnlyList<LineaVentaFinalizada> Lineas) : DomainEvent;