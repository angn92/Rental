using Newtonsoft.Json;
using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Account.Command.CreateAccount
{
    public class RegisterCustomer : ICommand
    {
        [JsonProperty("FirstName", Required = Required.Always)]
        public string FirstName { get; set; }

        [JsonProperty("LastName", Required = Required.Always)]
        public string LastName { get; set; }

        [JsonProperty("Username", Required = Required.Always)]
        public string Username { get; set; }

        [JsonProperty("Email", Required = Required.Always)]
        public string Email { get; set; }

        [JsonProperty("Password", Required = Required.Always)]
        public string Password { get; set; }
    }
}
