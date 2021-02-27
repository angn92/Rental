using System.Threading.Tasks;

namespace Rental.Infrastructure.Command
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}