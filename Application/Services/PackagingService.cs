using Core.DTOs;
using Core.DTOs.Packagings;
using Core.Entities;
using Core.Enums;
using Core.Helpers;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class PackagingService : IPackagingService
    {
        private readonly IRepository<PackagingUnit> _packagingRepository;
        private readonly IRepository<SerialNumber> _serialRepository;

        public PackagingService(IRepository<PackagingUnit> packagingRepository, IRepository<SerialNumber> serialRepository)
        {
            _packagingRepository = packagingRepository;
            _serialRepository = serialRepository;
        }

        public async Task<(PackagingResponseDto? Data, ResponseMessageDto Message)> CreatePackageAsync(PackagingCreateDto dto)
        {
            ResponseMessageDto responseMessage = new ResponseMessageDto();

            string baseSscc = "3" + "869123456" + DateTime.UtcNow.ToString("HHmmssf");
            string finalSscc = baseSscc + CalculateCheckDigit(baseSscc);

            PackagingUnit unit = new PackagingUnit
            {
                Sscc = finalSscc,
                PackagingLevelId = dto.PackagingLevelId
            };

            await _packagingRepository.AddAsync(unit);
            await _packagingRepository.SaveAsync();

            if (dto.PackagingLevelId == (int)PackagingLevel.Case)
            {
                var serials = await _serialRepository.GetWhere(x => dto.SerialNumberIds.Contains(x.Id)).ToListAsync();

                if (serials.Count == 0)
                {
                    responseMessage.Message = "Hata: Belirtilen ID'lere sahip ürünler bulunamadı.";
                    return (null, responseMessage);
                }

                foreach (var sn in serials) { sn.PackagingUnitId = unit.Id; }
            }
            else if (dto.PackagingLevelId == (int)PackagingLevel.Pallet)
            {
                var childPackages = await _packagingRepository.GetWhere(x => dto.SerialNumberIds.Contains(x.Id)).ToListAsync();

                if (childPackages.Count == 0)
                {
                    responseMessage.Message = "Hata: Pakete eklenecek geçerli alt birimler (koliler) bulunamadı.";
                    return (null, responseMessage);
                }

                foreach (var cp in childPackages) { cp.ParentId = unit.Id; }
            }
            await _packagingRepository.SaveAsync();

            responseMessage.Message = $"SSCC {finalSscc} ile agregasyon başarıyla tamamlandı.";
            var data = await GetPackageByIdAsync(unit.Id);
            return (data, responseMessage);
        }

        public async Task<List<PackagingResponseDto>> GetAllPackagesAsync()
        {
            var units = await _packagingRepository.Table
                .Include(x => x.SerialNumbers)
                .AsNoTracking()
                .ToListAsync();

            return units.Select(u => MapToDto(u)).ToList();
        }

        public async Task<PackagingResponseDto?> GetPackageByIdAsync(int id)
        {
            var unit = await _packagingRepository.Table
                .Include(x => x.SerialNumbers)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return unit != null ? MapToDto(unit) : null;
        }

        public async Task<ResponseMessageDto> UpdatePackageAsync(PackagingUpdateDto dto)
        {
            ResponseMessageDto responseMessage = new ResponseMessageDto();
            var unit = await _packagingRepository.GetByIdAsync(dto.Id);

            if (unit == null)
            {
                responseMessage.Message = "Paket bulunamadı.";
                return responseMessage;
            }

            var currentSerials = await _serialRepository.GetWhere(x => x.PackagingUnitId == unit.Id).ToListAsync();
            foreach (var cs in currentSerials) { cs.PackagingUnitId = null; _serialRepository.Update(cs); }

            var newSerials = await _serialRepository.GetWhere(x => dto.SerialNumberIds.Contains(x.Id)).ToListAsync();
            foreach (var ns in newSerials) { ns.PackagingUnitId = unit.Id; _serialRepository.Update(ns); }

            unit.PackagingLevelId = dto.PackagingLevelId;
            _packagingRepository.Update(unit);
            await _serialRepository.SaveAsync();
            await _packagingRepository.SaveAsync();

            responseMessage.Message = "Paket ve içeriği güncellendi.";
            return responseMessage;
        }

        public async Task<ResponseMessageDto> DeletePackageAsync(int id)
        {
            ResponseMessageDto responseMessage = new ResponseMessageDto();

            var serials = await _serialRepository.GetWhere(x => x.PackagingUnitId == id).ToListAsync();
            foreach (var sn in serials) { sn.PackagingUnitId = null; _serialRepository.Update(sn); }
            await _serialRepository.SaveAsync();

            bool result = await _packagingRepository.RemoveAsync(id);
            if (result) await _packagingRepository.SaveAsync();

            responseMessage.Message = result ? "Paket silindi, ürünler boşa çıkarıldı." : "Hata oluştu.";
            return responseMessage;
        }

        private PackagingResponseDto MapToDto(PackagingUnit entity)
        {
            var levelEnum = (PackagingLevel)entity.PackagingLevelId;
            var firstSerial = entity.SerialNumbers?.FirstOrDefault();

            return new PackagingResponseDto
            {
                Id = entity.Id,
                Sscc = entity.Sscc,
                PackagingLevelId = entity.PackagingLevelId,
                PackagingLevelName = levelEnum.GetDisplayName(),
                TotalItems = entity.SerialNumbers?.Count ?? 0,
                WorkOrderId = firstSerial?.WorkOrderId ?? 0,
                WorkOrderName = firstSerial?.WorkOrderId != null ? $"WO-{firstSerial.WorkOrderId}" : "N/A"
            };
        }

        private string CalculateCheckDigit(string data)
        {
            int sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                int digit = int.Parse(data[data.Length - 1 - i].ToString());
                sum += (i % 2 == 0) ? digit * 3 : digit * 1;
            }
            return ((10 - (sum % 10)) % 10).ToString();
        }
    }
}