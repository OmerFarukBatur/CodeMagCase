using Core.Entities.Common;

namespace Core.Entities
{
    public class Product : BaseEntity
    {
        
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Gtin { get; set; }

        
        public Customer Customer { get; set; }
        public ICollection<WorkOrder> WorkOrders { get; set; }
    }
}