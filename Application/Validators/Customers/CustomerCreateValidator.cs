using Core.DTOs.Customers;
using FluentValidation;

namespace Application.Validators.Customers
{
    public class CustomerCreateValidator : AbstractValidator<CustomerCreateDto>
    {
        public CustomerCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Firma Adı alanı zorunludur.");
            RuleFor(x => x.Gln).NotEmpty().WithMessage("GLN alanı zorunludur.").Length(13).WithMessage("GLN alanı 13 hane olmalıdır.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı zorunludur.");
        }
    }
}
