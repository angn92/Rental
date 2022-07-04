using Newtonsoft.Json;
using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Password
{
    public class ChangePasswordCommand : ICommand
    {
        [JsonProperty("Username", Required = Required.Always)]
        public string Username { get; set; }

        [JsonProperty("NewPassword", Required = Required.Always)]
        public string NewPassword { get; set; }
    }
}
