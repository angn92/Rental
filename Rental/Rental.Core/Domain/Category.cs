using JetBrains.Annotations;
using Rental.Core.Base;
using System.Collections.Generic;

namespace Rental.Core.Domain
{
    public class Category : Entity
    {
        public virtual string Name { get; protected set; }
        public virtual ISet<Product> Products { get; protected set; }
       
        protected Category()
        {}

        public Category([NotNull] string name)
        {
            Name = name;
        }
    }
}