using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Command
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        ValueTask HandleAsync(TCommand command, CancellationToken cancellationToken = default); 
    }

    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand
    {
        ValueTask<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    } 
}
