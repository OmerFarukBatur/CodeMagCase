using Core.DTOs;
using Core.DTOs.Products;

namespace Core.Interfaces.IServices
{
    public interface IProductService
    {
        Task<(ProductResponseDto? Data, ResponseMessageDto Message)> CreateProductAsync(ProductCreateDto dto);
        Task<List<ProductResponseDto>> GetAllProductsAsync();
        Task<ProductResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseMessageDto> UpdateProductAsync(ProductUpdateDto dto);
        Task<ResponseMessageDto> DeleteProductAsync(int id);
    }
}
