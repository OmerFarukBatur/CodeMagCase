using Core.DTOs.WorkOrders;
using Core.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkOrdersController : ControllerBase
    {
        private readonly IWorkOrderService _workOrderService;

        public WorkOrdersController(IWorkOrderService workOrderService)
        {
            _workOrderService = workOrderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _workOrderService.GetAllWorkOrdersAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _workOrderService.GetWorkOrderByIdAsync(id);
            return res != null ? Ok(res) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WorkOrderCreateDto dto)
        {
            var result = await _workOrderService.CreateWorkOrderAsync(dto);
            return result.Data != null ? Ok(result) : BadRequest(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] WorkOrderUpdateDto dto)
        {
            return Ok(await _workOrderService.UpdateWorkOrderAsync(dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _workOrderService.DeleteWorkOrderAsync(id));
        }

        [HttpGet("{id}/traceability")]
        public async Task<IActionResult> GetTraceability(int id)
        {
            var result = await _workOrderService.GetWorkOrderTraceabilityAsync(id);

            if (result == null)
                return NotFound(new { Message = "İş emri bulunamadı." });

            return Ok(result);
        }
    }
}
