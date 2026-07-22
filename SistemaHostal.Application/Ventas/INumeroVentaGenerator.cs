namespace SistemaHostal.Application.Ventas;

public interface INumeroVentaGenerator
{
    Task<string> GenerarAsync(CancellationToken cancellationToken = default);
}