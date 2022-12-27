using Newtonsoft.Json;

namespace Rental.Infrastructure.Handlers.Account.Command.CreateAccount
{
    public class RegisterCustomerResponse
    {
        [JsonProperty("SessionId")]
        public string SessionId { get; set; }
    }
}
