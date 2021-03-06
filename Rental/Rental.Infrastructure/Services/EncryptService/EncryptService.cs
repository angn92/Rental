﻿using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Rental.Infrastructure.Services.EncryptService
{
    public class EncryptService : IEncrypt
    {
        private static readonly int Size = 16;

        public string GetSalt(string password)
        {
            if (String.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Can not generate salt from empty argument", nameof(password));

            var salt = new byte[Size];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            return Convert.ToBase64String(salt);
        }

        public string GetHash(string password, string salt)
        {
            if (String.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Can not generate hash from empty argument", nameof(password));

            if (String.IsNullOrWhiteSpace(salt))
                throw new ArgumentException("Can not generate hash from empty argument", nameof(salt));

            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(password, GetBytes(salt), KeyDerivationPrf.HMACSHA512, 10000, 256 / 8));

            return hash;
        }

        private byte[] GetBytes(string value)
        {
            return Encoding.ASCII.GetBytes(value);
        }
    }
}
