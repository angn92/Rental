using Rental.Core.Validation;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Services.EncryptService;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Utils.Query
{
    public class PasswordHashHandler : IQueryHandler<PasswordHashQuery, PasswordHashResponse>
    {
        private readonly IEncrypt encrypt;

        public PasswordHashHandler(IEncrypt encrypt)
        {
            this.encrypt = encrypt;
        }

        public async ValueTask<PasswordHashResponse> HandleAsync(PasswordHashQuery query, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNullOrEmpty(query.Password, nameof(query.Password));
            ValidationParameter.FailIfNullOrEmpty(query.Salt, nameof(query.Salt));

            var salt = Convert.FromBase64String(query.Salt);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(query.Password),
                salt,
                350000,
                HashAlgorithmName.SHA512,
                128);

            return new PasswordHashResponse
            {
                Hash = Convert.ToHexString(hash)
            };
        }
    }
}
