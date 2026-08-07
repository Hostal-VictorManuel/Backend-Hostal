using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Ventas;

public record VentaTransferida(int VentaId, int UsuarioId, string HabitacionAnterior, string HabitacionNueva, string Motivo) : DomainEvent;