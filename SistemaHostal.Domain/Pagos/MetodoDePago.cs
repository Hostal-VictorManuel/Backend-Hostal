using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Pagos;

public class MetodoDePago : AggregateRoot
{
    protected MetodoDePago()
    {
        Nombre = string.Empty;
    }

    public MetodoDePago(string nombre) : this()
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del método de pago es obligatorio.", nameof(nombre));

        Nombre = nombre;
        Estado = EstadoMetodoDePago.Activo;
        FechaCreacion = DateTime.UtcNow;
    }

    public string Nombre { get; private set; }
    public EstadoMetodoDePago Estado { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime? FechaModificacion { get; private set; }

    public void Editar(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del método de pago es obligatorio.", nameof(nombre));

        Nombre = nombre;
        FechaModificacion = DateTime.UtcNow;
    }

    public void Activar()
    {
        Estado = EstadoMetodoDePago.Activo;
        FechaModificacion = DateTime.UtcNow;
    }

    public void Desactivar()
    {
        Estado = EstadoMetodoDePago.Inactivo;
        FechaModificacion = DateTime.UtcNow;
    }
}