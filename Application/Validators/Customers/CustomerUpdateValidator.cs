using Core.DTOs.Customers;
using FluentValidation;

namespace Application.Validators.Customers
{
    public class CustomerUpdateValidator : AbstractValidator<CustomerUpdateDto>
    {
        public CustomerUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Geçerli bir ID belirtilmelidir.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Firma Adı alanı zorunludur.");
            RuleFor(x => x.Gln).NotEmpty().WithMessage("GLN alanı zorunludur.").Length(13).WithMessage("GLN 13 hane olmalıdır.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı zorunludur.");
        }
    }
}
