namespace Rental.Infrastructure.Services.EncryptService
{
    public interface IEncrypt
    {
        string GenerateHash(string password, string salt);
        string GenerateSalt();
    }
}
