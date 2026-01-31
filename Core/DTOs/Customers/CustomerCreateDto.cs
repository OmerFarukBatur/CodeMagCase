namespace Core.DTOs.Customers
{
    public class CustomerCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Gln { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
