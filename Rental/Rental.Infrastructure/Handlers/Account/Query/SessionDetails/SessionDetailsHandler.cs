using Rental.Infrastructure.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Query.SessionDetails
{
    public class SessionDetailsHandler : IQueryHandler<SessionDetailsRq, SessionDetailsRs>
    {
        public ValueTask<SessionDetailsRs> HandleAsync(SessionDetailsRq query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
