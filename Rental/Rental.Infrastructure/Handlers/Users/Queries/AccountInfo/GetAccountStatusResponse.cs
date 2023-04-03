using Newtonsoft.Json;

namespace Rental.Infrastructure.Handlers.Users.Queries.AccountInfo
{
    public class GetAccountStatusResponse
    {

        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("AccountStatus")]
        public string AccountStatus { get; set; }

        [JsonProperty("PasswordStatus")]
        public string PasswordStatus { get; set; }
    }
}
