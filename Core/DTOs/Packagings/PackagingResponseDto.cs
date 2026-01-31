namespace Core.DTOs.Packagings
{
    public class PackagingResponseDto
    {
        public int Id { get; set; }
        public string Sscc { get; set; } = string.Empty;
        public int PackagingLevelId { get; set; }
        public string PackagingLevelName { get; set; } = string.Empty;
        public int? WorkOrderId { get; set; }
        public string WorkOrderName { get; set; } = string.Empty;
        public int TotalItems { get; set; }
    }
}
