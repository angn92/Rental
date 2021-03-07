using Rental.Core.Domain;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Services.EncryptService;

namespace Rental.Infrastructure.Helpers
{
    public interface IPasswordHelper
    {
        void SetPassword(string password, User user);
    }

    public class PasswordHelper : IPasswordHelper
    {
        private readonly IEncrypt _encrypt;
        private readonly RentalContext _context;

        public PasswordHelper(RentalContext context, IEncrypt encrypt)
        {
            _context = context;
            _encrypt = encrypt;
        }

        public void SetPassword(string password, User user)
        {
            var salt = _encrypt.GetSalt(password);
            var hash = _encrypt.GetHash(password, salt);

            var newPassword = new Password(hash, salt, user);

            _context.AddAsync(newPassword);
            _context.SaveChangesAsync();
        }
    }
}
