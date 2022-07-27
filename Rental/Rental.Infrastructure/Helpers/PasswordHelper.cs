using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Services.EncryptService;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Helpers
{
    public interface IPasswordHelper
    {
        Task SetPassword([NotNull] string password, [NotNull] Customer customer);
        Task<Password> GetActivePassword([NotNull] Customer customer);
        void ComaprePasswords([NotNull] Password currentPassword, [NotNull] string hashNewPassword);
        Task RemoveOldPassword([NotNull] string username);
    }

    public class PasswordHelper : IPasswordHelper
    {
        private readonly IEncrypt _encrypt;
        private readonly ApplicationDbContext _context;

        public PasswordHelper([NotNull] ApplicationDbContext context, [NotNull] IEncrypt encrypt)
        {
            _context = context;
            _encrypt = encrypt;
        }

        public void ComaprePasswords([NotNull] Password currentPassword, [NotNull] string hashNewPassword)
        {
            if (BCrypt.Net.BCrypt.Verify(hashNewPassword, currentPassword.Hash))
                throw new CoreException(ErrorCode.PasswordIncorrect, "Given password is incorrect.");
        }

        public async Task<Password> GetActivePassword([NotNull] Customer customer)
        {
            var password = await _context.Passwords.SingleOrDefaultAsync(x => x.Customer.Username == customer.Username 
                                                                        && x.Status == PasswordStatus.Active);

            if (password == null)
            {
                throw new CoreException(ErrorCode.PasswordNotExist, $"Customer {customer.Username} does not have active password.");
            }

            return password;
        }

        public async Task RemoveOldPassword([NotNull] string username)
        {
            var passwordToDelete = await _context.Passwords.SingleOrDefaultAsync(x => x.Customer.Username == username);

            if (passwordToDelete == null)
                return;

            _context.Passwords.Remove(passwordToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task SetPassword([NotNull] string password, [NotNull] Customer customer)
        {
            var salt = _encrypt.GetSalt(password);
            var hash = _encrypt.GetHash(password, salt);

            var newPassword = new Password(hash, salt, customer);

            await _context.AddAsync(newPassword);
            await _context.SaveChangesAsync();
        }
    }
}
