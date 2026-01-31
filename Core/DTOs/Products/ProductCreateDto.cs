namespace Core.DTOs.Products
{
    public class ProductCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Gtin { get; set; } = string.Empty;
        public int CustomerId { get; set; }
    }
}
