using Rental.Core.Domain;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Rental.Infrastructure.Services.EncryptService
{
    public class EncryptService : IEncrypt
    {
        const int keySize = 128;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public string HashCode(string code, byte[] salt)
        {
            var hashCode = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(code),
                salt,
                iterations,
                hashAlgorithm,
                keySize);

            return Convert.ToHexString(hashCode);
        }

        public string HashPasword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);

            return Convert.ToHexString(hash);
        }
    }
}
