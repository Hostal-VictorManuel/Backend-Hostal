using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Ventas;

public record EliminarLineaCommand(int VentaId, int LineaVentaId) : IRequest<Result<VentaDetalleDto>>;