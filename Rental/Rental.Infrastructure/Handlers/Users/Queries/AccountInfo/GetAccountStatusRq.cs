﻿using Newtonsoft.Json;
using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Users.Queries.AccountInfo
{
    public class GetAccountStatusRq : IQuery
    {
        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("SessionIdentifier")]
        public string SessionId { get; set; }
    }
}
