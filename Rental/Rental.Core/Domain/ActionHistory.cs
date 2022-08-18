using Rental.Core.Base;

namespace Rental.Core.Domain
{
    public class ActionHistory : Entity
    {
        public int Id { get; set; }

        /// <summary>
        /// Action name
        /// </summary>
        public string Name { get; set; }
    }
}
