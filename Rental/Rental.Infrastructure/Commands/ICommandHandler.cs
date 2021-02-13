using System.Threading.Tasks;

namespace Rental.Infrastructure.Commands
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command); 
    }
}
