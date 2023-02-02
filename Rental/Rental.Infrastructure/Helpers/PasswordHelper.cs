using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using Rental.Infrastructure.Services.EncryptService;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Helpers
{
    public interface IPasswordHelper
    {
        Task SetPassword([NotNull] string password, [NotNull] Customer customer, [NotNull] string code);
        Task<Password> GetActivePassword([NotNull] Customer customer);
        bool ComaprePasswords([NotNull] string currentPassword, [NotNull] string hashNewPassword);
        Task RemoveOldPassword([NotNull] string username);
        int GenerateActivationCode();
        Task<Password> FindPasswordToAuthorize([NotNull] string username);
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

        public bool ComaprePasswords([NotNull] string currentPassword, [NotNull] string newPassword)
        {
            return currentPassword.Equals(newPassword);
        }

        public async Task<Password> FindPasswordToAuthorize([NotNull] string username)
        {
            return await _context.Passwords.Where(x => x.Customer.Username == username && x.Status == PasswordStatus.NotActive)
                                           .OrderByDescending(x => x.CreatedAt)
                                           .FirstOrDefaultAsync();
        }

        public int GenerateActivationCode()
        {
            return RandomNumberGenerator.GetInt32(999999);
        }

        public async Task<Password> GetActivePassword([NotNull] Customer customer)
        {
            var password = await _context.Passwords.SingleOrDefaultAsync(x => x.Customer.Username == customer.Username 
                                                                        && x.Status == PasswordStatus.Active);

            if (password == null)
                throw new CoreException(ErrorCode.PasswordNotExist, $"Customer {customer.Username} does not have active password.");
            
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

        public async Task SetPassword([NotNull] string password, [NotNull] Customer customer, string code)
        {
            var hashPass = _encrypt.HashPasword(password, out var salt);
            var codeHash = _encrypt.HashCode(code, salt);
            var saltValue = Convert.ToBase64String(salt);

            var newPassword = new Password(hashPass, saltValue, customer, codeHash);

            await _context.AddAsync(newPassword);
            await _context.SaveChangesAsync();
        }
    }
}
