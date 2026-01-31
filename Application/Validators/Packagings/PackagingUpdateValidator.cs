using Core.DTOs.Packagings;
using FluentValidation;

namespace Application.Validators.Packagings
{
    public class PackagingUpdateValidator : AbstractValidator<PackagingUpdateDto>
    {
        public PackagingUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Geçerli bir paket ID'si zorunludur.");
            RuleFor(x => x.PackagingLevelId).InclusiveBetween(1, 2).WithMessage("Seviye 1 veya 2 olmalıdır.");
            RuleFor(x => x.SerialNumberIds).NotEmpty().WithMessage("Paket içeriği boş olamaz.");
        }
    }
}
