using Core.Entities.Common;
using Core.Enums;

namespace Core.Entities
{
    public class SerialNumber : BaseEntity
    {
        public int WorkOrderId { get; set; }       

        public string SerialValue { get; set; }
        public string Gs1FullString { get; set; }

        public int? PackagingUnitId { get; set; }
        
        public WorkOrder WorkOrder { get; set; }
        public PackagingUnit PackagingUnit { get; set; }
    }
}
