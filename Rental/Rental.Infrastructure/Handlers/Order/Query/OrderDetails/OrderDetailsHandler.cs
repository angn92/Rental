using Rental.Core.Validation;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Handlers.Order.Query.OrderDetails
{
    public class OrderDetailsHandler : IQueryHandler<OrderDetailsRq, OrderDetailsRs>
    {
        private readonly ApplicationDbContext context;

        public OrderDetailsHandler(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async ValueTask<OrderDetailsRs> HandleAsync(OrderDetailsRq query, CancellationToken cancellationToken = default)
        {
            ValidationParameter.FailIfNullOrEmpty(query.OrderId);
            ValidationParameter.FailIfNullOrEmpty(query.SessionId.ToString());
        }
    }
}
