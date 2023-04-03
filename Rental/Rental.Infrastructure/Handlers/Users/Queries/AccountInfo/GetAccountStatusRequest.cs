using Newtonsoft.Json;
using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Users.Queries.AccountInfo
{
    public class GetAccountStatusRequest : IQuery
    {
        [JsonProperty("Username")]
        public string Username { get; set; }
    }
}
