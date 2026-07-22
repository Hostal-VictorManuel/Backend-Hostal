using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Turnos;

public class Turno : AggregateRoot
{
    private readonly List<Incidencia> _incidencias = new();

    protected Turno()
    {
    }

    public Turno(int usuarioId) : this()
    {
        UsuarioId = usuarioId;
        FechaHoraInicio = DateTime.UtcNow;
        Estado = EstadoTurno.Activo;
    }

    public int UsuarioId { get; private set; }
    public DateTime FechaHoraInicio { get; private set; }
    public DateTime? FechaHoraFin { get; private set; }
    public EstadoTurno Estado { get; private set; }
    public IReadOnlyCollection<Incidencia> Incidencias => _incidencias.AsReadOnly();

    public void Finalizar()
    {
        if (Estado == EstadoTurno.Finalizado)
            throw new InvalidOperationException("El turno ya fue finalizado.");

        Estado = EstadoTurno.Finalizado;
        FechaHoraFin = DateTime.UtcNow;
    }

    public void RegistrarIncidencia(string descripcion)
    {
        if (Estado == EstadoTurno.Finalizado)
            throw new InvalidOperationException("No se pueden registrar incidencias en un turno finalizado.");

        _incidencias.Add(new Incidencia(descripcion));
    }
}