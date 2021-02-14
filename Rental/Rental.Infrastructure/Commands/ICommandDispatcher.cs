using System.Threading.Tasks;

namespace Rental.Infrastructure.Commands
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}