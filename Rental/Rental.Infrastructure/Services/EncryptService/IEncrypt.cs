namespace Rental.Infrastructure.Services.EncryptService
{
    public interface IEncrypt
    {
        string HashPasword(string password, out byte[] salt);
        string HashCode(string code, byte[] salt);
    }
}
