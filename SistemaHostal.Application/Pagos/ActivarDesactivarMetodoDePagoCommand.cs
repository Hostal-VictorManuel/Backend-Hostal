using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Pagos;

public record ActivarDesactivarMetodoDePagoCommand(int MetodoDePagoId, bool Activar) : IRequest<Result>;