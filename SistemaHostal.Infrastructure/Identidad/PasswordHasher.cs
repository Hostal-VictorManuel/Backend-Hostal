using SistemaHostal.Application.Identidad;

namespace SistemaHostal.Infrastructure.Identidad;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verificar(string password, string passwordHash) => BCrypt.Net.BCrypt.Verify(password, passwordHash);
}