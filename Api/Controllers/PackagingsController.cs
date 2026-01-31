using Core.DTOs.Packagings;
using Core.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackagingController : ControllerBase
    {
        private readonly IPackagingService _packagingService;

        public PackagingController(IPackagingService packagingService)
        {
            _packagingService = packagingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _packagingService.GetAllPackagesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _packagingService.GetPackageByIdAsync(id);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PackagingCreateDto dto)
        {
            var res = await _packagingService.CreatePackageAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PackagingUpdateDto dto)
        {
            return Ok(await _packagingService.UpdatePackageAsync(dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _packagingService.DeletePackageAsync(id));
        }
    }
}
