using Newtonsoft.Json;


namespace Rental.Infrastructure.Handlers.Password.Query
{
    public class PasswordHashResponse
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }
    }
}
