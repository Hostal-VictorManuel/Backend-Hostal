using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Catalogo;

public record ActivarDesactivarCategoriaCommand(int CategoriaId, bool Activar) : IRequest<Result>;