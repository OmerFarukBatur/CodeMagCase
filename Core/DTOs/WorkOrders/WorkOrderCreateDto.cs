namespace Core.DTOs.WorkOrders
{
    public class WorkOrderCreateDto
    {
        public int ProductId { get; set; }
        public int TargetQuantity { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public string SerialStartValue { get; set; } = string.Empty;
        public int WorkOrderStatusId { get; set; }
    }
}
