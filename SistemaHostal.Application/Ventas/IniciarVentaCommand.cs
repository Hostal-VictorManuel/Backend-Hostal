using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Ventas;

public record IniciarVentaCommand(int TurnoId, string? NumeroHabitacion, int? TrabajadorId) : IRequest<Result<VentaResumenDto>>;