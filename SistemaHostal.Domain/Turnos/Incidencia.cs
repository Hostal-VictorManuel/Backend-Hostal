using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Turnos;

public class Incidencia : Entity
{
    protected Incidencia()
    {
        Descripcion = string.Empty;
    }

    public Incidencia(string descripcion) : this()
    {
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException("La descripción de la incidencia es obligatoria.", nameof(descripcion));

        Descripcion = descripcion;
        FechaHora = DateTime.UtcNow;
    }

    public string Descripcion { get; private set; }
    public DateTime FechaHora { get; private set; }
    public int TurnoId { get; private set; }
}