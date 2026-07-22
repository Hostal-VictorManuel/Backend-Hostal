namespace SistemaHostal.Application.Reportes;

public record FlujoCajaMensualDto(int Anio, int Mes, decimal TotalIngresos, IReadOnlyList<IngresoDiarioDto> IngresosPorDia);