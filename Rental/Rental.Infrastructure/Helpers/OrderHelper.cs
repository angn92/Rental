using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Helpers
{
    public interface IOrderHelper
    {
        Task<Order> GetAcceptedOrderForGivenProduct([NotNull] ApplicationDbContext context, [NotNull] string productId,
                                [NotNull] string borrower);
    }

    public class OrderHelper : IOrderHelper
    {
        public async Task<Order> GetAcceptedOrderForGivenProduct([NotNull] ApplicationDbContext context, [NotNull] string productId,
                        [NotNull] string borrower)
        {
            var order = await context.Transactions.SingleOrDefaultAsync(x => x.ProductId == productId && x.CustomerId == borrower);

            if (order is null && order.OrderStatus != OrderStatus.Accepted)
                throw new CoreException(ErrorCode.OrderNotExist, $"Given order not exist.");

            return order;
        }
    }
}
