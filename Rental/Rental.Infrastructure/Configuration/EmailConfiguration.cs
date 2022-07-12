using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace Rental.Infrastructure.Configuration
{
    public sealed class EmailConfiguration
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
