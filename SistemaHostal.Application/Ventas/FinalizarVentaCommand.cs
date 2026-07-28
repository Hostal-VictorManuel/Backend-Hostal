using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Ventas;

public record FinalizarVentaCommand(int VentaId, IReadOnlyList<PagoInput> Pagos, bool CargarAHabitacion, bool CargarATrabajador, int UsuarioId) : IRequest<Result<VentaDetalleDto>>;