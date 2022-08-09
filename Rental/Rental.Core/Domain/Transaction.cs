using Rental.Core.Base;
using System;

namespace Rental.Core.Domain
{
    public class Transaction : Entity
    {
        public virtual string TransactionId { get; set; }
        public virtual string ProductId { get; set; }
        public virtual string CustomerId { get; set; }
        public virtual string ProductName { get; set; }
        public virtual string TransactionHash { get; set; }
        public virtual DateTime? From { get; set; }
        public virtual DateTime? To { get; set; }
        public virtual int Amount { get; set; }

       
    }
}
