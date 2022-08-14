using Newtonsoft.Json;
using Rental.Core.Enum;
using Rental.Infrastructure.Command;
using System;

namespace Rental.Infrastructure.Handlers.Product.Command.Cancel
{
    public class CancelReservationCommand : ICommand
    {
        [JsonProperty("productId", Required = Required.Always)]
        public string ProductId { get; set; }

        /// <summary>
        /// User name who canceled reservation
        /// </summary>
        [JsonProperty("username", Required = Required.Always)]
        public string Username { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("cancelTime", Required = Required.Always)]
        public DateTime? CancelTime { get; set; }

        [JsonProperty("status", Required = Required.Always)]
        public ProductStatus Status { get; set; }
    }
}
