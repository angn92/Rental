using System.Threading.Tasks;

namespace Rental.Infrastructure.Commands
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<T>(T command) where T : ICommand;
    }
}
