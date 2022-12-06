using Newtonsoft.Json;

namespace Rental.Infrastructure.Handlers.Account.Command.CreateAccount
{
    public class RegisterCustomerResponse
    {
        [JsonProperty("sessionId")]
        public string SessionId { get; set; }
    }
}
