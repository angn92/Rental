using Rental.Infrastructure.Command;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Account.Command.LoginSession
{
    public class AuthenticationSessionHandler : ICommandHandler<AuthenticationSessionCommand, AuthenticationSessionResponse>
    {
        public ValueTask<AuthenticationSessionResponse> HandleAsync(AuthenticationSessionCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
