namespace SistemaHostal.Application.Identidad;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verificar(string password, string passwordHash);
}