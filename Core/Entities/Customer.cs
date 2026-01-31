using Core.Entities.Common;

namespace Core.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string Gln { get; set; }
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}