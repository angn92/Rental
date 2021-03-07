namespace Rental.Infrastructure.Services.EncryptService
{
    public interface IEncrypt
    {
        string GetHash(string password, string salt);
        string GetSalt(string password);
    }
}
