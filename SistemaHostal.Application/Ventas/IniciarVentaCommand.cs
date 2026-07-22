using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Ventas;

public record IniciarVentaCommand(int TurnoId, string? NumeroHabitacion) : IRequest<Result<VentaResumenDto>>;