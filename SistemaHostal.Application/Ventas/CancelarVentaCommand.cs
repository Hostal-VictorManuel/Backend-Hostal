using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Ventas;

public record CancelarVentaCommand(int VentaId) : IRequest<Result>;