using Core.DTOs;
using Core.DTOs.Customers;

namespace Core.Interfaces.IServices
{
    public interface ICustomerService
    {
        Task<(CustomerResponseDto? Data, ResponseMessageDto Message)> CreateCustomerAsync(CustomerCreateDto customerCreateDto);
        Task<List<CustomerResponseDto>> GetAllCustomersAsync();
        Task<CustomerResponseDto?> GetCustomerByIdAsync(int id);
        Task<ResponseMessageDto> UpdateCustomerAsync(CustomerUpdateDto customerUpdateDto);
        Task<ResponseMessageDto> DeleteCustomerAsync(int id);
    }
}
