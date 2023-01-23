using Rental.Core.Validation;
using Rental.Infrastructure.Query;
using Rental.Infrastructure.Services.EncryptService;
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

            var hash = encrypt.GenerateHash(query.Password, query.Salt);

            return new PasswordHashResponse
            {
                Hash = hash
            };
        }
    }
}
