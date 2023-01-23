using Newtonsoft.Json;
using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Utils.Query
{
    public class PasswordHashQuery : IQuery
    {
        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("Salt")]
        public string Salt { get; set; }
    }
}
