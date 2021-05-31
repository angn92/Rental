using Rental.Infrastructure.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Users.Command.AccountStatus
{
    public class ChangeAccountStatus : ICommand
    {
        public string Username { get; set; }
    }
}
