namespace SistemaHostal.Application.Pagos;

public interface IMetodoDePagoQueries
{
    Task<IReadOnlyList<MetodoDePagoDto>> BuscarAsync(string? texto, CancellationToken cancellationToken = default);
}