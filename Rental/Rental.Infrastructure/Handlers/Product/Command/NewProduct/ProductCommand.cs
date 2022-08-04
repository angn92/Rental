﻿using Newtonsoft.Json;
using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Product.Command.NewProduct
{
    public class ProductCommand : ICommand
    {
        public ProductCommand(ProductRequest request)
        {
            Request = request;
        }
        public ProductRequest Request { get; }
    }

    public class ProductRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}