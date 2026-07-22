using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Ventas;

public class LineaVenta : Entity
{
    protected LineaVenta()
    {
        NombreProducto = string.Empty;
    }

    public LineaVenta(int productoId, string nombreProducto, decimal precioUnitario, int cantidad) : this()
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(cantidad));

        ProductoId = productoId;
        NombreProducto = nombreProducto;
        PrecioUnitario = precioUnitario;
        Cantidad = cantidad;
    }

    public int ProductoId { get; private set; }
    public string NombreProducto { get; private set; }
    public decimal PrecioUnitario { get; private set; }
    public int Cantidad { get; private set; }
    public decimal Subtotal => PrecioUnitario * Cantidad;
    public int VentaId { get; private set; }

    public void CambiarCantidad(int nuevaCantidad)
    {
        if (nuevaCantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(nuevaCantidad));

        Cantidad = nuevaCantidad;
    }

    internal void IncrementarCantidad(int cantidad)
    {
        Cantidad += cantidad;
    }
}