using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Ventas;

public record TransferirHabitacionCommand(int VentaId, string NumeroHabitacionNueva, string Motivo, int UsuarioId) : IRequest<Result<VentaDetalleDto>>;