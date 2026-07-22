using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Catalogo;

public class Categoria : AggregateRoot
{
    protected Categoria()
    {
        // Requerido por EF Core
        Nombre = string.Empty;
    }

    public Categoria(string nombre) : this()
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre de la categoría es obligatorio.", nameof(nombre));

        Nombre = nombre;
        Estado = EstadoCategoria.Activa;
        FechaCreacion = DateTime.UtcNow;
    }

    public string Nombre { get; private set; }
    public EstadoCategoria Estado { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime? FechaModificacion { get; private set; }

    public void Editar(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre de la categoría es obligatorio.", nameof(nombre));

        Nombre = nombre;
        FechaModificacion = DateTime.UtcNow;
    }

    public void Activar()
    {
        Estado = EstadoCategoria.Activa;
        FechaModificacion = DateTime.UtcNow;
    }

    public void Desactivar()
    {
        Estado = EstadoCategoria.Inactiva;
        FechaModificacion = DateTime.UtcNow;
    }
}