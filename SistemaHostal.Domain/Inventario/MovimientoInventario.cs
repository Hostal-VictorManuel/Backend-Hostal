using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Inventario;

public class MovimientoInventario : AggregateRoot
{
    protected MovimientoInventario()
    {
    }

    public MovimientoInventario(int productoId, TipoMovimiento tipo, int cantidad, int usuarioId, string? motivo, int? ventaId) : this()
    {
        if (cantidad == 0)
            throw new ArgumentException("La cantidad del movimiento no puede ser cero.", nameof(cantidad));

        ProductoId = productoId;
        Tipo = tipo;
        Cantidad = cantidad;
        UsuarioId = usuarioId;
        Motivo = motivo;
        VentaId = ventaId;
        FechaHora = DateTime.UtcNow;
    }

    public int ProductoId { get; private set; }
    public TipoMovimiento Tipo { get; private set; }
    public int Cantidad { get; private set; }
    public int UsuarioId { get; private set; }
    public string? Motivo { get; private set; }
    public int? VentaId { get; private set; }
    public DateTime FechaHora { get; private set; }
}