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
        public string Name { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public string CategoryName { get; set; }
    }
}
