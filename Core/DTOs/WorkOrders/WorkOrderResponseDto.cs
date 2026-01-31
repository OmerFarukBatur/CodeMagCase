namespace Core.DTOs.WorkOrders
{
    public class WorkOrderResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TargetQuantity { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public string SerialStartValue { get; set; } = string.Empty;
        public int WorkOrderStatusId { get; set; }
        public string WorkOrderStatusName { get; set; } = string.Empty;
    }
}
