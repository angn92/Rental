﻿using Newtonsoft.Json;
using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Password
{
    public class ChangePasswordCommand : ICommand
    {
        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("NewPassword")]
        public string NewPassword { get; set; }
    }
}
