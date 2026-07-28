using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Trabajadores;

public record RegistrarTrabajadorCommand(string Nombre) : IRequest<Result<TrabajadorDto>>;