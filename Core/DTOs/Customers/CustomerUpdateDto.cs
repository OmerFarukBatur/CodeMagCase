namespace Core.DTOs.Customers
{
    public class CustomerUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Gln { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
