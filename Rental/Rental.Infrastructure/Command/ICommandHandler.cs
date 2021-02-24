using System.Threading.Tasks;

namespace Rental.Infrastructure.Command
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command); 
    }

    public interface ICommandHandler<TQuery, TResult> 
        where TQuery : ICommand 
        where TResult : class
    {
        Task<TResult> HandleAsync(TQuery commad);
    }
}
