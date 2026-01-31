using Core.Entities.Common;
using Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class WorkOrder : BaseEntity
    {
        public int ProductId { get; set; }       

        public int TargetQuantity { get; set; }
        public string LotNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string SerialStartValue { get; set; }

        public int WorkOrderStatusId { get; set; }

        [NotMapped]
        public WorkOrderStatus WorkOrderStatus
        {
            get => (WorkOrderStatus)WorkOrderStatusId;
            set => WorkOrderStatusId = (int)value;
        }        
        
        public Product Product { get; set; }
        public ICollection<SerialNumber> SerialNumbers { get; set; }
    }
}