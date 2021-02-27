using Rental.Infrastructure.Query;

namespace Rental.Infrastructure.Handlers.Users.Queries
{
    public class GetUserDetailsRq : IQuery
    {
        public string Nick { get; set; }
    }
}
