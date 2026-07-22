using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Catalogo;

public class Producto : AggregateRoot
{
    protected Producto()
    {
        // Requerido por EF Core
        CodigoBarras = string.Empty;
        Nombre = string.Empty;
    }

    public Producto(
        string codigoBarras,
        string nombre,
        int categoriaId,
        decimal precio,
        int stockInicial,
        int stockMinimo,
        string? imagenUrl) : this()
    {
        if (string.IsNullOrWhiteSpace(codigoBarras))
            throw new ArgumentException("El código de barras es obligatorio.", nameof(codigoBarras));
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre es obligatorio.", nameof(nombre));
        if (precio <= 0)
            throw new ArgumentException("El precio debe ser mayor a cero.", nameof(precio));
        if (stockInicial < 0)
            throw new ArgumentException("El stock inicial no puede ser negativo.", nameof(stockInicial));
        if (stockMinimo < 0)
            throw new ArgumentException("El stock mínimo no puede ser negativo.", nameof(stockMinimo));

        CodigoBarras = codigoBarras;
        Nombre = nombre;
        CategoriaId = categoriaId;
        Precio = precio;
        StockActual = stockInicial;
        StockMinimo = stockMinimo;
        ImagenUrl = imagenUrl;
        Estado = EstadoProducto.Activo;
        FechaCreacion = DateTime.UtcNow;
    }

    public string CodigoBarras { get; private set; }
    public string Nombre { get; private set; }
    public int CategoriaId { get; private set; }
    public decimal Precio { get; private set; }
    public int StockActual { get; private set; }
    public int StockMinimo { get; private set; }
    public string? ImagenUrl { get; private set; }
    public EstadoProducto Estado { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime? FechaModificacion { get; private set; }

    public void Editar(string nombre, int categoriaId, decimal precio, int stockMinimo, string? imagenUrl)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre es obligatorio.", nameof(nombre));
        if (precio <= 0)
            throw new ArgumentException("El precio debe ser mayor a cero.", nameof(precio));
        if (stockMinimo < 0)
            throw new ArgumentException("El stock mínimo no puede ser negativo.", nameof(stockMinimo));

        Nombre = nombre;
        CategoriaId = categoriaId;
        Precio = precio;
        StockMinimo = stockMinimo;
        ImagenUrl = imagenUrl;
        FechaModificacion = DateTime.UtcNow;
    }

    public void Activar()
    {
        Estado = EstadoProducto.Activo;
        FechaModificacion = DateTime.UtcNow;
    }

    public void Desactivar()
    {
        Estado = EstadoProducto.Inactivo;
        FechaModificacion = DateTime.UtcNow;
    }

    public void IncrementarStock(int cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad a ingresar debe ser mayor a cero.", nameof(cantidad));

        StockActual += cantidad;
        FechaModificacion = DateTime.UtcNow;
    }

    public void AjustarStock(int nuevoStock)
    {
        if (nuevoStock < 0)
            throw new ArgumentException("El stock no puede ser negativo.", nameof(nuevoStock));

        StockActual = nuevoStock;
        FechaModificacion = DateTime.UtcNow;
    }

    public void DescontarStock(int cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad a descontar debe ser mayor a cero.", nameof(cantidad));
        if (cantidad > StockActual)
            throw new InvalidOperationException("No hay stock suficiente para descontar esa cantidad.");

        StockActual -= cantidad;
        FechaModificacion = DateTime.UtcNow;
    }

    public bool TieneStockCritico() => StockActual <= StockMinimo;

    public bool TieneStockSuficiente(int cantidad) => StockActual >= cantidad;
}