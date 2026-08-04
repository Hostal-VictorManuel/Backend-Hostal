using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Ventas;

public record VentaAnulada(int VentaId, int UsuarioId, string Motivo, IReadOnlyList<LineaVentaFinalizada> Lineas) : DomainEvent;