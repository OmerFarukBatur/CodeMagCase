using Core.DTOs;
using Core.DTOs.Products;
using Core.Entities;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }
        
        public async Task<List<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.Table
                .Include(x => x.Customer)
                .AsNoTracking()
                .ToListAsync();

            List<ProductResponseDto> responseList = new List<ProductResponseDto>();
            foreach (var item in products)
            {
                responseList.Add(MapToDto(item));
            }
            return responseList;
        }

        public async Task<(ProductResponseDto? Data, ResponseMessageDto Message)> CreateProductAsync(ProductCreateDto dto)
        {
            ResponseMessageDto responseMessage = new ResponseMessageDto();

            Product product = new Product
            {
                Name = dto.Name,
                Gtin = dto.Gtin,
                CustomerId = dto.CustomerId
            };

            await _productRepository.AddAsync(product);
            int result = await _productRepository.SaveAsync();

            if (result > 0)
            {
                responseMessage.Message = "Ürün başarıyla oluşturuldu.";
                var data = await GetProductByIdAsync(product.Id);
                return (data, responseMessage);
            }

            responseMessage.Message = "Ürün oluşturulurken teknik bir hata oluştu.";
            return (null, responseMessage);
        }        

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.Table
                .Include(x => x.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return product != null ? MapToDto(product) : null;
        }

        public async Task<ResponseMessageDto> UpdateProductAsync(ProductUpdateDto dto)
        {
            ResponseMessageDto responseMessage = new ResponseMessageDto();

            Product? product = await _productRepository.GetByIdAsync(dto.Id);

            if (product == null)
            {
                responseMessage.Message = "Güncellenecek ürün bulunamadı.";
                return responseMessage;
            }

            product.Name = dto.Name;
            product.Gtin = dto.Gtin;
            product.CustomerId = dto.CustomerId;

            _productRepository.Update(product);
            int result = await _productRepository.SaveAsync();

            responseMessage.Message = result > 0 ? "Ürün başarıyla güncellendi." : "Değişiklik yapılmadı.";
            return responseMessage;
        }

        public async Task<ResponseMessageDto> DeleteProductAsync(int id)
        {
            ResponseMessageDto responseMessage = new ResponseMessageDto();

            bool isRemoved = await _productRepository.RemoveAsync(id);
            if (!isRemoved)
            {
                responseMessage.Message = "Silinecek ürün bulunamadı.";
                return responseMessage;
            }

            int result = await _productRepository.SaveAsync();
            responseMessage.Message = result > 0 ? "Ürün başarıyla silindi." : "Silme işlemi başarısız.";
            return responseMessage;
        }

        private ProductResponseDto MapToDto(Product entity)
        {
            return new ProductResponseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Gtin = entity.Gtin,
                CustomerId = entity.CustomerId,
                CustomerName = entity.Customer?.Name ?? "Firma Bilgisi Yok"
            };
        }
    }
}
