namespace SistemaHostal.Application.Trabajadores;

public interface ITrabajadorQueries
{
    Task<IReadOnlyList<TrabajadorDto>> BuscarAsync(string? texto, CancellationToken cancellationToken = default);
}