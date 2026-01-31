using Core.DTOs;
using Core.DTOs.Packagings;

namespace Core.Interfaces.IServices
{
    public interface IPackagingService
    {
        Task<(PackagingResponseDto? Data, ResponseMessageDto Message)> CreatePackageAsync(PackagingCreateDto dto);
        Task<List<PackagingResponseDto>> GetAllPackagesAsync();
        Task<PackagingResponseDto?> GetPackageByIdAsync(int id);
        Task<ResponseMessageDto> UpdatePackageAsync(PackagingUpdateDto dto);
        Task<ResponseMessageDto> DeletePackageAsync(int id);
    }
}
