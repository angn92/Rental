using Newtonsoft.Json;
using Rental.Core.Enum;

namespace Rental.Infrastructure.Handlers.Account.Query.AccountDetails
{
    public class GetCustomerDetailsResponse
    {
        [JsonProperty("FullName")]
        public string Fullname { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Status")]
        public AccountStatus Status { get; set; }
    }
}
