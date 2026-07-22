using SistemaHostal.Domain.Common;

namespace SistemaHostal.Domain.Identidad;

public class Usuario : AggregateRoot
{
    protected Usuario()
    {
        // Requerido por EF Core
        NombreCompleto = string.Empty;
        NombreUsuario = string.Empty;
        PasswordHash = string.Empty;
    }

    public Usuario(string nombreCompleto, string nombreUsuario, string passwordHash, RolUsuario rol) : this()
    {
        if (string.IsNullOrWhiteSpace(nombreCompleto))
            throw new ArgumentException("El nombre completo es obligatorio.", nameof(nombreCompleto));
        if (string.IsNullOrWhiteSpace(nombreUsuario))
            throw new ArgumentException("El nombre de usuario es obligatorio.", nameof(nombreUsuario));
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("El password es obligatorio.", nameof(passwordHash));

        NombreCompleto = nombreCompleto;
        NombreUsuario = nombreUsuario;
        PasswordHash = passwordHash;
        Rol = rol;
        Estado = EstadoUsuario.Activo;
        FechaCreacion = DateTime.UtcNow;
    }

    public string NombreCompleto { get; private set; }
    public string NombreUsuario { get; private set; }
    public string PasswordHash { get; private set; }
    public RolUsuario Rol { get; private set; }
    public EstadoUsuario Estado { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime? FechaModificacion { get; private set; }
    public DateTime? UltimoAcceso { get; private set; }

    public void Editar(string nombreCompleto, string nombreUsuario, RolUsuario rol)
    {
        if (string.IsNullOrWhiteSpace(nombreCompleto))
            throw new ArgumentException("El nombre completo es obligatorio.", nameof(nombreCompleto));
        if (string.IsNullOrWhiteSpace(nombreUsuario))
            throw new ArgumentException("El nombre de usuario es obligatorio.", nameof(nombreUsuario));

        NombreCompleto = nombreCompleto;
        NombreUsuario = nombreUsuario;
        Rol = rol;
        FechaModificacion = DateTime.UtcNow;
    }

    public void Activar()
    {
        Estado = EstadoUsuario.Activo;
        FechaModificacion = DateTime.UtcNow;
    }

    public void Desactivar()
    {
        Estado = EstadoUsuario.Inactivo;
        FechaModificacion = DateTime.UtcNow;
    }

    public void RestablecerPassword(string nuevoPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(nuevoPasswordHash))
            throw new ArgumentException("El password es obligatorio.", nameof(nuevoPasswordHash));

        PasswordHash = nuevoPasswordHash;
        FechaModificacion = DateTime.UtcNow;
    }

    public void RegistrarAcceso()
    {
        UltimoAcceso = DateTime.UtcNow;
    }

    public bool PuedeIniciarSesion() => Estado == EstadoUsuario.Activo;
}