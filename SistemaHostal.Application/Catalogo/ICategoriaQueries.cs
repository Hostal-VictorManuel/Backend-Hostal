namespace SistemaHostal.Application.Catalogo;

public interface ICategoriaQueries
{
    Task<IReadOnlyList<CategoriaResumenDto>> BuscarAsync(string? texto, CancellationToken cancellationToken = default);
}