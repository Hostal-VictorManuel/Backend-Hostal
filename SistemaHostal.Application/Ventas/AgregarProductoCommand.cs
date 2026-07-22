using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Ventas;

public record AgregarProductoCommand(int VentaId, int ProductoId, int Cantidad) : IRequest<Result<VentaDetalleDto>>;