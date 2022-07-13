using JetBrains.Annotations;
using Rental.Infrastructure.Configuration;

namespace Rental.Test.Helpers
{
    public class EmailTestHelper
    {
        public static EmailConfiguration PrepareEmail([NotNull] string from, [NotNull] string to, [NotNull] string subject, [NotNull] string message)
        {
            var email = new EmailConfiguration
            {
                From = from,
                To = to,
                Subject = subject,
                Message = message
            };

            return email;
        }
    }
}
