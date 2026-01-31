using Core.DTOs;
using Core.DTOs.WorkOrders;

namespace Core.Interfaces.IServices
{
    public interface IWorkOrderService
    {
        Task<(WorkOrderResponseDto? Data, ResponseMessageDto Message)> CreateWorkOrderAsync(WorkOrderCreateDto dto);
        Task<List<WorkOrderResponseDto>> GetAllWorkOrdersAsync();
        Task<WorkOrderResponseDto?> GetWorkOrderByIdAsync(int id);
        Task<ResponseMessageDto> UpdateWorkOrderAsync(WorkOrderUpdateDto dto);
        Task<ResponseMessageDto> DeleteWorkOrderAsync(int id);

        Task<WorkOrderTraceabilityResponseDto?> GetWorkOrderTraceabilityAsync(int id);
    }
}
