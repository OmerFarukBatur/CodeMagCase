using Core.DTOs;
using Core.DTOs.Customers;
using Core.Entities;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<CustomerResponseDto>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAll(tracking: false).ToListAsync();

            List<CustomerResponseDto> responseList = new List<CustomerResponseDto>();

            foreach (var item in customers)
            {
                CustomerResponseDto dto = new CustomerResponseDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Gln = item.Gln,
                    Description = item.Description
                };
                responseList.Add(dto);
            }

            return responseList;
        }

        public async Task<(CustomerResponseDto? Data, ResponseMessageDto Message)> CreateCustomerAsync(CustomerCreateDto customerCreateDto)
        {
            ResponseMessageDto responseMessage = new ResponseMessageDto();

            Customer customerEntity = new Customer
            {
                Name = customerCreateDto.Name,
                Gln = customerCreateDto.Gln,
                Description = customerCreateDto.Description
            };

            await _customerRepository.AddAsync(customerEntity);
            int result = await _customerRepository.SaveAsync();

            if (result > 0)
            {
                responseMessage.Message = "Müşteri başarıyla oluşturuldu.";

                CustomerResponseDto responseData = new CustomerResponseDto
                {
                    Id = customerEntity.Id,
                    Name = customerEntity.Name,
                    Gln = customerEntity.Gln,
                    Description = customerEntity.Description
                };

                return (responseData, responseMessage);
            }

            responseMessage.Message = "Müşteri oluşturulurken bir hata oluştu.";
            return (null, responseMessage);
        }

        public async Task<CustomerResponseDto?> GetCustomerByIdAsync(int id)
        {
            Customer? customer = await _customerRepository.GetByIdAsync(id, tracking: false);

            if (customer == null)
            {
                return null;
            }
            CustomerResponseDto responseDto = new CustomerResponseDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Gln = customer.Gln,
                Description = customer.Description
            };

            return responseDto;
        }

        public async Task<ResponseMessageDto> UpdateCustomerAsync(CustomerUpdateDto customerUpdateDto)
        {
            ResponseMessageDto responseMessage = new ResponseMessageDto();

            Customer? customer = await _customerRepository.GetByIdAsync(customerUpdateDto.Id);

            if (customer == null)
            {
                responseMessage.Message = "Güncellenecek müşteri bulunamadı.";
                return responseMessage;
            }

            customer.Name = customerUpdateDto.Name;
            customer.Gln = customerUpdateDto.Gln;
            customer.Description = customerUpdateDto.Description;

            _customerRepository.Update(customer);
            int result = await _customerRepository.SaveAsync();

            if (result > 0)
            {
                responseMessage.Message = "Müşteri başarıyla güncellendi.";
            }
            else
            {
                responseMessage.Message = "Güncelleme sırasında bir değişiklik yapılmadı veya hata oluştu.";
            }

            return responseMessage;
        }

        public async Task<ResponseMessageDto> DeleteCustomerAsync(int id)
        {
            ResponseMessageDto responseMessage = new ResponseMessageDto();
            bool isRemoved = await _customerRepository.RemoveAsync(id);

            if (!isRemoved)
            {
                responseMessage.Message = "Silinecek müşteri bulunamadı.";
                return responseMessage;
            }

            int result = await _customerRepository.SaveAsync();

            if (result > 0)
            {
                responseMessage.Message = "Müşteri başarıyla silindi.";
            }
            else
            {
                responseMessage.Message = "Silme işlemi sırasında bir hata oluştu.";
            }

            return responseMessage;
        }
    }
}
