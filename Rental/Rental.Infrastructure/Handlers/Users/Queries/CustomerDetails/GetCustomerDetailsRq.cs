using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Users.Queries
{
    public class GetCustomerDetailsRq : IQuery
    {
        public string Nick { get; set; }
    }
}
