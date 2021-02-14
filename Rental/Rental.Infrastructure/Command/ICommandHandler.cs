using System.Threading.Tasks;

namespace Rental.Infrastructure.Command
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command); 
    }
}
