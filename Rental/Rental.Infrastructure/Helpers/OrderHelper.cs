﻿using JetBrains.Annotations;
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
        Task<Order> GetAcceptedOrderForGivenProduct([NotNull] string productId, [NotNull] string borrower);
        Task<Order> GetOrderByIdAsync([NotNull] string orderId);
        List<Order> GetActiveOrders([NotNull] string custmerId);
        List<Order> GetFinishedOrderForCustomer([NotNull] string customer);
    }

    public class OrderHelper : IOrderHelper
    {
        private readonly ApplicationDbContext _context;

        public OrderHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetAcceptedOrderForGivenProduct([NotNull] string productId, [NotNull] string borrower)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(x => x.ProductId == productId && x.CustomerId == borrower);

            if (order is null)
                throw new CoreException(ErrorCode.OrderNotExist, $"Order with number {order.OrderId} does not exist.");

            if (order.OrderStatus != OrderStatus.Accepted)
                throw new CoreException(ErrorCode.OrderWasFinishedOrCancelled, string.Format($"Order {order.OrderId} has status: {order.OrderStatus}"));
            
            return order;
        }

        public List<Order> GetActiveOrders([NotNull] string custmerId)
        {
            return _context.Orders.Where(x => x.CustomerId == custmerId && x.OrderStatus == OrderStatus.Accepted).ToList();
        }

        public List<Order> GetFinishedOrderForCustomer([NotNull] string username)
        {
            return _context.Orders.Where(x => x.CustomerId == username && x.OrderStatus == OrderStatus.Finished).ToList();
        }

        public async Task<Order> GetOrderByIdAsync([NotNull] string orderId)
        {
            return await _context.Orders.SingleOrDefaultAsync(x => x.OrderId == orderId);
        }


    }
}
