using SistemaHostal.Domain.Catalogo;

namespace SistemaHostal.Application.Catalogo;


public interface IProductoQueries
{
    Task<IReadOnlyList<ProductoResumenDto>> BuscarAsync(
        string? texto, int? categoriaId, EstadoProducto? estado, CancellationToken cancellationToken = default);

    Task<ProductoDetalleDto?> ObtenerDetalleAsync(int productoId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductoResumenDto>> ObtenerConStockCriticoAsync(CancellationToken cancellationToken = default);
}