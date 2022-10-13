using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using MimeKit;
using Rental.Infrastructure.Configuration;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rental.Infrastructure.Helpers
{
    public interface IEmailHelper
    {
        void ValidateEmail([NotNull] string email);
        void SendEmail([NotNull] EmailConfiguration emailConfiguration);
    }

    public class EmailHelper : IEmailHelper
    {
        private readonly ApplicationDbContext _context;
        private readonly IOptions<ConfigurationOptions> _options;

        public EmailHelper(ApplicationDbContext context, IOptions<ConfigurationOptions> options)
        {
            _context = context;
            _options = options;
        }

        public void SendEmail([NotNull] EmailConfiguration emailConfiguration)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(MailboxAddress.Parse(emailConfiguration.From));
            emailMessage.To.Add(MailboxAddress.Parse(emailConfiguration.To));
            emailMessage.Subject = emailConfiguration.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = emailConfiguration.Message };

            if (!_options.Value.SendRealEmail)
                return;

            //Send real email
        }

        public void ValidateEmail([NotNull] string email)
        {
            if (String.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

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
