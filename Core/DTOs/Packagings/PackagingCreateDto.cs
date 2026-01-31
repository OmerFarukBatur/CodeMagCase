namespace Core.DTOs.Packagings
{
    public class PackagingCreateDto
    {
        public int WorkOrderId { get; set; }
        public int PackagingLevelId { get; set; }
        public List<int> SerialNumberIds { get; set; } = new List<int>();
    }
}
