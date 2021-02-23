using System.Threading.Tasks;

namespace Rental.Infrastructure.Command
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
        Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command) 
            where TCommand : ICommand
            where TResult : class;
    }
}