namespace SistemaHostal.Application.Trabajadores;

public record TrabajadorDto(int Id, string Nombre, string Estado, DateTime FechaCreacion, DateTime? FechaModificacion);