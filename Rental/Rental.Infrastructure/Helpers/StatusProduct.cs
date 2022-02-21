using Rental.Core.Enum;

namespace Rental.Infrastructure.Helpers
{
    public class StatusProduct
    {
        public string Name { get; set; }
        public int AvailableQuantity { get; set; }
        public ProductStatus Status { get; set; }
    }
}