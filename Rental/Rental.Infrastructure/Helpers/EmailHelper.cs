using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rental.Infrastructure.Helpers
{
    public interface IEmailValidator
    {
        void ValidateEmail(string email);
    }

    public class EmailHelper : IEmailValidator
    {
        private readonly ApplicationDbContext _context;

        public EmailHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public void ValidateEmail(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
                throw new Exception($"Argument {nameof(email)} can not be null or empty.");

            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var match = regex.Match(email);

            if (!match.Success)
                throw new CoreException(ErrorCode.InvalidEmail, $"Address email {email} is incorrect");

            IsEmailInUse(_context, email);
        }

        private void IsEmailInUse(ApplicationDbContext context, string email)
        {
            var emailExist = context.Customers.SingleOrDefault(x => x.Email == email);

            if (emailExist != null)
                throw new CoreException(ErrorCode.EmailInUse, $"Given address email {email} is in use.");
        }
    }
}
