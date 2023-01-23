using Newtonsoft.Json;

namespace Rental.Infrastructure.Handlers.Utils.Query
{
    public class PasswordHashResponse
    {
        [JsonProperty("Hash")]
        public string Hash { get; set; }
    }
}
