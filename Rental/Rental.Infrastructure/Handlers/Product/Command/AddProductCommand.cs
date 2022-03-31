using Newtonsoft.Json;
using Rental.Infrastructure.Command;

namespace Rental.Infrastructure.Handlers.Product.Command
{
    public class AddProductCommand : ICommand
    {
        public AddProductCommand(ProductRequest request)
        {
            Request = request;
        }

        public ProductRequest Request { get; }
    }

    public class ProductRequest
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Amount")]
        public int Amount { get; set; }

        [JsonProperty("CategoryName")]
        public string CategoryName { get; set; }
    }
}
