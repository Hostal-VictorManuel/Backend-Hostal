using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Pagos;

public record RegistrarMetodoDePagoCommand(string Nombre) : IRequest<Result<MetodoDePagoDto>>;