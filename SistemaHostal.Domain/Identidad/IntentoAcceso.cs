using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Identidad;

public class IntentoAcceso : AggregateRoot
{
    protected IntentoAcceso()
    {
        NombreUsuarioIntentado = string.Empty;
        Ip = string.Empty;
    }

    public IntentoAcceso(string nombreUsuarioIntentado, string ip, ResultadoAcceso resultado) : this()
    {
        NombreUsuarioIntentado = nombreUsuarioIntentado;
        Ip = ip;
        Resultado = resultado;
        FechaHora = DateTime.UtcNow;
    }

    public string NombreUsuarioIntentado { get; private set; }
    public string Ip { get; private set; }
    public ResultadoAcceso Resultado { get; private set; }
    public DateTime FechaHora { get; private set; }
}