using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Rental.Core.Domain;
using Rental.Core.Enum;
using Rental.Infrastructure.EF;
using Rental.Infrastructure.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rental.Infrastructure.Helpers
{
    public interface IOrderHelper
    {
        Task<Order> GetAcceptedOrderForGivenProduct([NotNull] ApplicationDbContext context, [NotNull] string productId, [NotNull] string borrower);

        Task<Order> GetOrderByIdAsync([NotNull] ApplicationDbContext context, [NotNull] string orderId);

        List<Order> GetActiveOrders([NotNull] ApplicationDbContext context, [NotNull] string custmerId);

        List<Order> GetFinishedOrderForCustomer([NotNull] ApplicationDbContext context, [NotNull] string customer);
    }

    public class OrderHelper : IOrderHelper
    {
        public async Task<Order> GetAcceptedOrderForGivenProduct([NotNull] ApplicationDbContext context, [NotNull] string productId, 
            [NotNull] string borrower)
        {
            var order = await context.Orders.SingleOrDefaultAsync(x => x.ProductId == productId && x.CustomerId == borrower);

            if (order is null)
                throw new CoreException(ErrorCode.OrderNotExist, $"Order with number {order.OrderId} does not exist.");

            if (order.OrderStatus != OrderStatus.Accepted)
                throw new CoreException(ErrorCode.OrderWasFinishedOrCancelled, string.Format($"Order {order.OrderId} has status: {order.OrderStatus}"));
            
            return order;
        }

        public List<Order> GetActiveOrders([NotNull] ApplicationDbContext context, [NotNull] string custmerId)
        {
            return context.Orders.Where(x => x.CustomerId == custmerId && x.OrderStatus == OrderStatus.Accepted).ToList();
        }

        public List<Order> GetFinishedOrderForCustomer([NotNull] ApplicationDbContext context, [NotNull] string username)
        {
            return context.Orders.Where(x => x.CustomerId == username && x.OrderStatus == OrderStatus.Finished).ToList();
        }

        public async Task<Order> GetOrderByIdAsync([NotNull] ApplicationDbContext context, [NotNull] string orderId)
        {
            return await context.Orders.SingleOrDefaultAsync(x => x.OrderId == orderId);
        }


    }
}
