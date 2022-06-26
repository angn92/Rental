using Newtonsoft.Json;
using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Account.Query.AccountDetails
{
    public class GetCustomerDetailsRq : IQuery
    {
        [JsonProperty("username", Required = Required.Always)]
        public string Username { get; set; }
    }
}
