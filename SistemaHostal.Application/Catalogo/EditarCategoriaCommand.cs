using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Catalogo;

public record EditarCategoriaCommand(int CategoriaId, string Nombre) : IRequest<Result<CategoriaResumenDto>>;