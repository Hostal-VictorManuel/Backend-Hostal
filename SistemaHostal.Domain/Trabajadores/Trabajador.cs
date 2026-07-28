using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Trabajadores;

public class Trabajador : AggregateRoot
{
    protected Trabajador()
    {
        Nombre = string.Empty;
    }

    public Trabajador(string nombre) : this()
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del trabajador es obligatorio.", nameof(nombre));

        Nombre = nombre;
        Estado = EstadoTrabajador.Activo;
        FechaCreacion = DateTime.UtcNow;
    }

    public string Nombre { get; private set; }
    public EstadoTrabajador Estado { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime? FechaModificacion { get; private set; }

    public void Editar(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del trabajador es obligatorio.", nameof(nombre));

        Nombre = nombre;
        FechaModificacion = DateTime.UtcNow;
    }

    public void Activar()
    {
        Estado = EstadoTrabajador.Activo;
        FechaModificacion = DateTime.UtcNow;
    }

    public void Desactivar()
    {
        Estado = EstadoTrabajador.Inactivo;
        FechaModificacion = DateTime.UtcNow;
    }
}