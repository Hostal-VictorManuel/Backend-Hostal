using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Auditoria;

public class RegistroBitacora : AggregateRoot
{
    protected RegistroBitacora()
    {
        NombreUsuario = string.Empty;
        Detalle = string.Empty;
    }

    public RegistroBitacora(
        int usuarioId,
        string nombreUsuario,
        ModuloAuditoria modulo,
        TipoOperacionAuditoria tipoOperacion,
        string detalle) : this()
    {
        UsuarioId = usuarioId;
        NombreUsuario = nombreUsuario;
        Modulo = modulo;
        TipoOperacion = tipoOperacion;
        Detalle = detalle;
        FechaHora = DateTime.UtcNow;
    }

    public int UsuarioId { get; private set; }
    public string NombreUsuario { get; private set; }
    public ModuloAuditoria Modulo { get; private set; }
    public TipoOperacionAuditoria TipoOperacion { get; private set; }
    public string Detalle { get; private set; }
    public DateTime FechaHora { get; private set; }
}