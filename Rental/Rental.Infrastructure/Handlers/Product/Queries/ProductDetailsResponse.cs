namespace Rental.Infrastructure.Handlers.Product.Queries
{
    public class ProductDetailsResponse
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }
        public string Category { get; set; }
        public string Owner { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
        Available = 1,
        InUse
    }
}