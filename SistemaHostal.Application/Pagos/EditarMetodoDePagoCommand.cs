using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Pagos;

public record EditarMetodoDePagoCommand(int MetodoDePagoId, string Nombre) : IRequest<Result<MetodoDePagoDto>>;