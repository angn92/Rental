using JetBrains.Annotations;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Helpers
{
    public interface IEmailHelper
    {
        void ValidateEmail([NotNull] string email);
        Task SendEmail([NotNull] string receiverEmail, [NotNull] string subject, [CanBeNull] string content = null);
    }

    public class EmailHelper : IEmailHelper
    {
        private readonly ApplicationDbContext _context;

        public EmailHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task SendEmail([NotNull] string receiverEmail, [NotNull] string subject, [CanBeNull] string content = null)
        {
            throw new NotImplementedException();
        }

        public void ValidateEmail([NotNull] string email)
        {
            if (String.IsNullOrWhiteSpace(email))
                throw new Exception($"Argument {nameof(email)} can not be null or empty.");

            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var match = regex.Match(email);

            if (!match.Success)
                throw new CoreException(ErrorCode.InvalidEmail, $"Address email {email} is incorrect");

            IsEmailInUse(_context, email);
        }

        private static void IsEmailInUse([NotNull] ApplicationDbContext context, [NotNull] string email)
        {
            var emailExist = context.Customers.SingleOrDefault(x => x.Email == email);

            if (emailExist != null)
                throw new CoreException(ErrorCode.EmailInUse, $"Given address email {email} is in use.");
        }
    }
}
