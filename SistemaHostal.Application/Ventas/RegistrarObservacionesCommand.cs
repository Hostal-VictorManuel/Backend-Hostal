using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Ventas;

public record RegistrarObservacionesCommand(int VentaId, string? Observaciones) : IRequest<Result>;