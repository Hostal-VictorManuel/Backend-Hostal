using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Identidad;

public record LoginCommand(string NombreUsuario, string Password, string Ip) : IRequest<Result<LoginResponseDto>>;