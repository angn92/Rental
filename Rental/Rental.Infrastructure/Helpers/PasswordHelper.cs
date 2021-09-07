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
        Task SetPassword(string password, Customer user);
        Task<Password> GetActivePassword(Customer user);
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

        public async Task<Password> GetActivePassword(Customer user)
        {
            var password = await _context.Passwords.SingleOrDefaultAsync(x => x.User.Username == user.Username && 
                                                              x.Status == PasswordStatus.Active);

            if(password == null)
            {
                throw new CoreException(ErrorCode.PasswordNotExist, $"User {user.Username} does not have active password.");
            }

            return password;
        }

        public async Task SetPassword(string password, Customer user)
        {
            var salt = _encrypt.GetSalt(password);
            var hash = _encrypt.GetHash(password, salt);

            var newPassword = new Password(hash, salt, user);

            await _context.AddAsync(newPassword);
            await _context.SaveChangesAsync();
        }
    }
}
