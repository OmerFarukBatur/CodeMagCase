namespace Core.DTOs.WorkOrders
{
    public class WorkOrderTraceabilityResponseDto
    {
        public int WorkOrderId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Gtin { get; set; } = string.Empty;
        public string LotNumber { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public int TargetQuantity { get; set; }

        public List<SerialNumberDetailDto> Serials { get; set; } = new List<SerialNumberDetailDto>();
    }

    public class SerialNumberDetailDto
    {
        public string SerialValue { get; set; } = string.Empty;
        public string Gs1FullString { get; set; } = string.Empty;

        public string? CaseSscc { get; set; } 
        public string? PalletSscc { get; set; }
    }
}
