using Rental.Core.Base;

namespace Rental.Core.Domain
{
    public class Dictionary : Entity
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int DictionaryId { get; set; }

        /// <summary>
        /// Dictionary name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Dictioanry value
        /// </summary>
        public virtual string Value { get; set; }
    }
}
