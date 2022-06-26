using Newtonsoft.Json;
using Rental.Core.Enum;

namespace Rental.Infrastructure.Handlers.Account.Query.AccountDetails
{
    public class GetCustomerDetailsRs
    {
        [JsonProperty("fullName")]
        public string Fullname { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("status")]
        public AccountStatus Status { get; set; }
    }
}
