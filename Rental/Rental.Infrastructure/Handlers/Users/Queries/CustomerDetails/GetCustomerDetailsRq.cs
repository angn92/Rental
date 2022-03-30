using Newtonsoft.Json;
using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Users.Queries
{
    public class GetCustomerDetailsRq : IQuery
    {
        [JsonProperty("Username", Required = Required.Always)]
        public string Nick { get; set; }
    }
}
