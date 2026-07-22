namespace SistemaHostal.Application.Inventario;

public interface IMovimientoInventarioQueries
{
    Task<IReadOnlyList<MovimientoInventarioDto>> ListarAsync(
        int? productoId, DateTime? fecha, CancellationToken cancellationToken = default);
}