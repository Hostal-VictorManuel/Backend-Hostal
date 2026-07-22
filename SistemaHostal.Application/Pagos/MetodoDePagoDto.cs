namespace SistemaHostal.Application.Pagos;

public record MetodoDePagoDto(
    int Id,
    string Nombre,
    string Estado,
    DateTime FechaCreacion,
    DateTime? FechaModificacion);