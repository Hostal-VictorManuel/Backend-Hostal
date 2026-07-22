using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Catalogo;

public record ActivarDesactivarProductoCommand(int ProductoId, bool Activar) : IRequest<Result>;