using Core.DTOs.Packagings;
using FluentValidation;

namespace Application.Validators.Packagings
{
    public class PackagingCreateValidator : AbstractValidator<PackagingCreateDto>
    {
        public PackagingCreateValidator()
        {
            RuleFor(x => x.PackagingLevelId).InclusiveBetween(1, 2).WithMessage("Seviye 1 (Koli) veya 2 (Palet) olmalıdır.");
            RuleFor(x => x.SerialNumberIds).NotEmpty().WithMessage("Paketlenecek en az bir ürün seçilmelidir.");
        }
    }
}
