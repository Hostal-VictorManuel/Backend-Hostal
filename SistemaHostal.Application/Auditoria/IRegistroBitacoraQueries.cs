using SistemaHostal.Domain.Auditoria;

namespace SistemaHostal.Application.Auditoria;

public interface IRegistroBitacoraQueries
{
    Task<IReadOnlyList<RegistroBitacoraDto>> BuscarAsync(
        int? usuarioId,
        DateTime? fecha,
        TipoOperacionAuditoria? tipoOperacion,
        ModuloAuditoria? modulo,
        CancellationToken cancellationToken = default);
}