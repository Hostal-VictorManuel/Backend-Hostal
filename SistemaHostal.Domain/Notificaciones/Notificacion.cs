using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Notificaciones;

public class Notificacion : AggregateRoot
{
    protected Notificacion()
    {
        Canal = string.Empty;
        Contenido = string.Empty;
    }

    public Notificacion(string canal, string contenido) : this()
    {
        if (string.IsNullOrWhiteSpace(canal))
            throw new ArgumentException("El canal es obligatorio.", nameof(canal));
        if (string.IsNullOrWhiteSpace(contenido))
            throw new ArgumentException("El contenido es obligatorio.", nameof(contenido));

        Canal = canal;
        Contenido = contenido;
        Estado = EstadoNotificacion.NoLeida;
        FechaRecepcion = DateTime.UtcNow;
    }

    public string Canal { get; private set; }
    public string Contenido { get; private set; }
    public EstadoNotificacion Estado { get; private set; }
    public DateTime FechaRecepcion { get; private set; }

    public void MarcarComoLeida()
    {
        Estado = EstadoNotificacion.Leida;
    }
}