using Autofac;
using System;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Command
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IComponentContext _context;

        public CommandDispatcher(IComponentContext context)
        {
            _context = context;
        }

        public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            if(command == null)
            {
                throw new Exception($"Command {command} can not be null.");
            }

            var handler = _context.Resolve<ICommandHandler<TCommand>>();
            await handler.HandleAsync(command);
        }
    }
}
