using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Catalogo;

public record RegistrarCategoriaCommand(string Nombre) : IRequest<Result<CategoriaResumenDto>>;