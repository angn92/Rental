
using Rental.Infrastructure.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Users.Queries.AccountInfo
{
    public class GetAccountStatusRq : IQuery
    {
        public string Username { get; set; }
        public string IdSession { get; set; }
    }
}
