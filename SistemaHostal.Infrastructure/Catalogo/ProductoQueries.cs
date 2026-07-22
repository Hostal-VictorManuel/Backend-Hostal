using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Catalogo;
using SistemaHostal.Domain.Catalogo;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Catalogo;

public class ProductoQueries(SistemaHostalDbContext context) : IProductoQueries
{
    public async Task<IReadOnlyList<ProductoResumenDto>> BuscarAsync(
        string? texto, int? categoriaId, EstadoProducto? estado, CancellationToken cancellationToken = default)
    {
        var query =
            from p in context.Set<Producto>().AsNoTracking()
            join c in context.Set<Categoria>().AsNoTracking() on p.CategoriaId equals c.Id
            select new { Producto = p, Categoria = c };

        if (!string.IsNullOrWhiteSpace(texto))
            query = query.Where(x => x.Producto.Nombre.Contains(texto) || x.Producto.CodigoBarras.Contains(texto));

        if (categoriaId.HasValue)
            query = query.Where(x => x.Producto.CategoriaId == categoriaId.Value);

        if (estado.HasValue)
            query = query.Where(x => x.Producto.Estado == estado.Value);

        return await query
            .Select(x => new ProductoResumenDto(
                x.Producto.Id, x.Producto.CodigoBarras, x.Producto.Nombre, x.Categoria.Nombre,
                x.Producto.Precio, x.Producto.StockActual, x.Producto.StockMinimo, x.Producto.Estado.ToString()))
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductoDetalleDto?> ObtenerDetalleAsync(int productoId, CancellationToken cancellationToken = default)
    {
        return await (
            from p in context.Set<Producto>().AsNoTracking()
            join c in context.Set<Categoria>().AsNoTracking() on p.CategoriaId equals c.Id
            where p.Id == productoId
            select new ProductoDetalleDto(
                p.Id, p.CodigoBarras, p.Nombre, p.CategoriaId, c.Nombre,
                p.Precio, p.StockActual, p.StockMinimo, p.ImagenUrl, p.Estado.ToString(), p.FechaCreacion)
        ).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProductoResumenDto>> ObtenerConStockCriticoAsync(CancellationToken cancellationToken = default)
    {
        var query =
            from p in context.Set<Producto>().AsNoTracking()
            join c in context.Set<Categoria>().AsNoTracking() on p.CategoriaId equals c.Id
            where p.StockActual <= p.StockMinimo && p.Estado == EstadoProducto.Activo
            select new ProductoResumenDto(
                p.Id, p.CodigoBarras, p.Nombre, c.Nombre,
                p.Precio, p.StockActual, p.StockMinimo, p.Estado.ToString());

        return await query.ToListAsync(cancellationToken);
    }
}