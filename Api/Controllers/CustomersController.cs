using Application.Services;
using Core.DTOs.Customers;
using Core.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }        

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _customerService.GetAllCustomersAsync();
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerCreateDto customerCreateDto)
        {
            var result = await _customerService.CreateCustomerAsync(customerCreateDto);

            if (result.Data != null)
            {
                return Ok(new { Data = result.Data, Info = result.Message });
            }

            return BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _customerService.GetCustomerByIdAsync(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CustomerUpdateDto customerUpdateDto)
        {
            if (customerUpdateDto == null) return BadRequest();

            var result = await _customerService.UpdateCustomerAsync(customerUpdateDto);

            if (result.Message.Contains("başarıyla"))
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);

            if (result.Message.Contains("başarıyla"))
            {
                return Ok(result);
            }

            return NotFound(result);
        }
    }
}
