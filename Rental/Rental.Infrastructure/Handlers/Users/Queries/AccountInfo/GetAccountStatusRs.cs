using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Users.Queries.AccountInfo
{
    public class GetAccountStatusRs
    {
        [JsonProperty("AccountStatus")]
        public string AccountStatus { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }
    }
}
