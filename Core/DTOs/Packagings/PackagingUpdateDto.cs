namespace Core.DTOs.Packagings
{
    public class PackagingUpdateDto
    {
        public int Id { get; set; }
        public int PackagingLevelId { get; set; }
        public List<int> SerialNumberIds { get; set; } = new List<int>();
    }
}
