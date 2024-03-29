﻿using Newtonsoft.Json;
using Rental.Core.Enum;
using System;

namespace Rental.Infrastructure.Handlers.Account.Command.Sessions
{
    public class CreateSessionResponse
    {
        [JsonProperty("IdSession")]
        public string IdSession { get; set; }

        [JsonProperty("Status")]
        public SessionState Status { get; set; }

        [JsonProperty("ExpirationTime")]
        public DateTime? ExpirationTime { get; set; }
    }
}
