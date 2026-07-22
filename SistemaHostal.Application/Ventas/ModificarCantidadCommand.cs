using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Ventas;

public record ModificarCantidadCommand(int VentaId, int LineaVentaId, int NuevaCantidad) : IRequest<Result<VentaDetalleDto>>;