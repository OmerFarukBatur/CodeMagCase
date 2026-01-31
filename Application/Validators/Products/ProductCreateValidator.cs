using Core.DTOs.Products;
using FluentValidation;

namespace Application.Validators.Products
{
    public class ProductCreateValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün Adı alanı zorunludur.");

            RuleFor(x => x.Gtin)
                .NotEmpty().WithMessage("GTIN alanı zorunludur.")
                .Length(14).WithMessage("GTIN tam olarak 14 hane olmalıdır.")
                .Matches(@"^[0-9]+$").WithMessage("GTIN sadece rakamlardan oluşmalıdır.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Geçerli bir müşteri seçilmelidir.");
        }
    }
}
