﻿using Newtonsoft.Json;
using Rental.Core.Enum;
using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Account.Command
{
    public class ChangeStatusCommand : ICommand
    {
        public ChangeStatusRequest ChangeStatusRequest { get; }

        public ChangeStatusCommand(ChangeStatusRequest request)
        {
            ChangeStatusRequest = request;
        }
    }

    public class ChangeStatusRequest
    {
        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("AccountStatus")]
        public AccountStatus Status { get; set; }
    }
}
