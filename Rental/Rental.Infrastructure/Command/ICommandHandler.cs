using System.Threading.Tasks;

namespace Rental.Infrastructure.Command
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command); 
    }

    public interface ICommandHandler<TCommad, TResult> 
        where TCommad : ICommand 
        where TResult : class
    {
        Task<TResult> HandleAsync(TCommad commad);
    }
}
