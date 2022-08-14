using JetBrains.Annotations;
using Rental.Core.Base;
using Rental.Core.Enum;
using System;

namespace Rental.Core.Domain
{
    public class Order : Entity
    {
        public virtual string OrderId { get; set; }
        public virtual string ProductId { get; set; }

        /// <summary>
        /// Id person who has done reservation
        /// </summary>
        public virtual string CustomerId { get; set; }
        public virtual string ProductName { get; set; }
        public virtual string OrderHash { get; set; }
        public virtual DateTime? From { get; set; }
        public virtual DateTime? To { get; set; }
        public virtual int Amount { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }

        protected Order()
        {
        }

        public Order(string orderId, string productId, string customerId, string productName, string orderHash, DateTime? from, DateTime? to, 
                    int amount, OrderStatus orderStatus)
        {
            OrderId = orderId;
            ProductId = productId;
            CustomerId = customerId;
            ProductName = productName;
            OrderHash = orderHash;
            From = from;
            To = to;
            Amount = amount;
            OrderStatus = orderStatus;
        }

        public void ChangeOrderStatus([NotNull] OrderStatus status)
        {
            OrderStatus = status;
        }
    }
}
