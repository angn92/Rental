using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Command
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, CancellationToken cancellationToken = default); 
    }

    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand
    {
        Task<TResult> HandleAsync(TCommand command);
    } 
}
