using Newtonsoft.Json;

namespace Rental.Infrastructure.Handlers.Account.Command.CreateAccount
{
    public class RegisterCustomerResponse
    {
        [JsonProperty("SessionIdentifier")]
        public string SessionId { get; set; }
    }
}
