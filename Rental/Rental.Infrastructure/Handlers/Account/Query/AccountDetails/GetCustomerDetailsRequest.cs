using Newtonsoft.Json;
using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Account.Query.AccountDetails
{
    public class GetCustomerDetailsRequest : IQuery
    {
        [JsonProperty("Username")]
        public string Username { get; set; }
    }
}
