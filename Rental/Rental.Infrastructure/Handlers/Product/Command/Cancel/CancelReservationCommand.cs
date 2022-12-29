using Newtonsoft.Json;
using Rental.Infrastructure.Command;
using System;

namespace Rental.Infrastructure.Handlers.Product.Command.Cancel
{
    public class CancelReservationCommand : ICommand
    {
        [JsonProperty("ProductId", Required = Required.Always)]
        public string ProductId { get; set; }

        /// <summary>
        /// User name who canceled reservation
        /// </summary>
        [JsonProperty("Username", Required = Required.Always)]
        public string Username { get; set; }

        [JsonProperty("Reason")]
        public string Reason { get; set; }

        [JsonProperty("CancelTime", Required = Required.Always)]
        public DateTime? CancelationTime { get; set; }
    }
}
