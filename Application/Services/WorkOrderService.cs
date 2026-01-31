using Core.DTOs;
using Core.DTOs.WorkOrders;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IRepository<WorkOrder> _workOrderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<SerialNumber> _serialNumberRepository;
        private readonly ILogger<WorkOrderService> _logger;

        public WorkOrderService(
            IRepository<WorkOrder> workOrderRepository,
            IRepository<Product> productRepository,
            IRepository<SerialNumber> serialNumberRepository,
            ILogger<WorkOrderService> logger)
        {
            _workOrderRepository = workOrderRepository;
            _productRepository = productRepository;
            _serialNumberRepository = serialNumberRepository;
            _logger = logger;
        }

        public async Task<(WorkOrderResponseDto? Data, ResponseMessageDto Message)> CreateWorkOrderAsync(WorkOrderCreateDto dto)
        {
            ResponseMessageDto responseMessage = new ResponseMessageDto();

            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
            {
                responseMessage.Message = "Hata: Belirtilen ürün bulunamadı.";
                return (null, responseMessage);
            }

            WorkOrder workOrder = new WorkOrder
            {
                ProductId = dto.ProductId,
                TargetQuantity = dto.TargetQuantity,
                LotNumber = dto.LotNumber,
                ExpiryDate = dto.ExpiryDate,
                SerialStartValue = dto.SerialStartValue,
                WorkOrderStatusId = dto.WorkOrderStatusId
            };

            await _workOrderRepository.AddAsync(workOrder);
            await _workOrderRepository.SaveAsync();

            List<SerialNumber> serials = new List<SerialNumber>();

            for (int i = 1; i <= dto.TargetQuantity; i++)
            {
                string currentSerial = string.Empty;
                bool isUniqueInDb = false;
                int suffix = i;

                while (!isUniqueInDb)
                {
                    currentSerial = $"{dto.SerialStartValue}{suffix.ToString("D5")}";

                    var exists = await _serialNumberRepository.Table
                        .AnyAsync(x => x.SerialValue == currentSerial && x.WorkOrder.ProductId == product.Id);

                    if (!exists)
                    {
                        isUniqueInDb = true;
                    }
                    else
                    {
                        suffix++;
                    }
                }

                var sn = new SerialNumber
                {
                    WorkOrderId = workOrder.Id,
                    SerialValue = currentSerial,
                    Gs1FullString = $"(01){product.Gtin}(21){currentSerial}(17){dto.ExpiryDate:yyMMdd}(10){dto.LotNumber}"
                };

                serials.Add(sn);

                _logger.LogInformation("Serial Generated: {Gs1}", sn.Gs1FullString);
            }

            await _serialNumberRepository.AddRangeAsync(serials);
            await _serialNumberRepository.SaveAsync();

            responseMessage.Message = $"{dto.TargetQuantity} adet GS1-128 seri numarası ile iş emri başarıyla oluşturuldu.";

            var data = await GetWorkOrderByIdAsync(workOrder.Id);
            return (data, responseMessage);
        }


        public async Task<List<WorkOrderResponseDto>> GetAllWorkOrdersAsync()
        {
            var orders = await _workOrderRepository.Table
                .Include(x => x.Product)
                .AsNoTracking()
                .ToListAsync();

            return orders.Select(MapToDto).ToList();
        }

        public async Task<WorkOrderResponseDto?> GetWorkOrderByIdAsync(int id)
        {
            var order = await _workOrderRepository.Table
                .Include(x => x.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return order != null ? MapToDto(order) : null;
        }

        public async Task<ResponseMessageDto> UpdateWorkOrderAsync(WorkOrderUpdateDto dto)
        {
            ResponseMessageDto responseMessage = new ResponseMessageDto();
            WorkOrder? workOrder = await _workOrderRepository.GetByIdAsync(dto.Id);

            if (workOrder == null)
            {
                responseMessage.Message = "İş emri bulunamadı.";
                return responseMessage;
            }

            workOrder.ProductId = dto.ProductId;
            workOrder.TargetQuantity = dto.TargetQuantity;
            workOrder.LotNumber = dto.LotNumber;
            workOrder.ExpiryDate = dto.ExpiryDate;
            workOrder.SerialStartValue = dto.SerialStartValue;
            workOrder.WorkOrderStatusId = dto.WorkOrderStatusId;

            _workOrderRepository.Update(workOrder);
            int result = await _workOrderRepository.SaveAsync();

            responseMessage.Message = result > 0 ? "İş emri güncellendi." : "Değişiklik yapılmadı.";
            return responseMessage;
        }

        public async Task<ResponseMessageDto> DeleteWorkOrderAsync(int id)
        {
            ResponseMessageDto responseMessage = new ResponseMessageDto();
            bool result = await _workOrderRepository.RemoveAsync(id);

            if (result)
            {
                await _workOrderRepository.SaveAsync();
                responseMessage.Message = "İş emri silindi.";
            }
            else
            {
                responseMessage.Message = "Kayıt bulunamadı.";
            }

            return responseMessage;
        }

        private WorkOrderResponseDto MapToDto(WorkOrder entity)
        {
            var statusEnum = (WorkOrderStatus)entity.WorkOrderStatusId;

            return new WorkOrderResponseDto
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                ProductName = entity.Product?.Name ?? "Ürün Belirtilmemiş",
                TargetQuantity = entity.TargetQuantity,
                LotNumber = entity.LotNumber,
                ExpiryDate = entity.ExpiryDate,
                SerialStartValue = entity.SerialStartValue,
                WorkOrderStatusId = entity.WorkOrderStatusId,
                WorkOrderStatusName = statusEnum.ToString()
            };
        }

        public async Task<WorkOrderTraceabilityResponseDto?> GetWorkOrderTraceabilityAsync(int id)
        {
            var order = await _workOrderRepository.Table
                .Include(x => x.Product)
                .Include(x => x.SerialNumbers)
                    .ThenInclude(s => s.PackagingUnit)
                        .ThenInclude(p => p.Parent)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null) return null;

            var statusEnum = (WorkOrderStatus)order.WorkOrderStatusId;

            return new WorkOrderTraceabilityResponseDto
            {
                WorkOrderId = order.Id,
                ProductName = order.Product?.Name ?? "N/A",
                Gtin = order.Product?.Gtin ?? "N/A",
                LotNumber = order.LotNumber,
                TargetQuantity = order.TargetQuantity,
                StatusName = statusEnum.ToString(),
                Serials = order.SerialNumbers.Select(s => new SerialNumberDetailDto
                {
                    SerialValue = s.SerialValue,
                    Gs1FullString = s.Gs1FullString,
                    CaseSscc = s.PackagingUnit?.Sscc,
                    PalletSscc = s.PackagingUnit?.Parent?.Sscc
                }).ToList()
            };
        }
    }
}