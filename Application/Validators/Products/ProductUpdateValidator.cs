using Core.DTOs.Products;
using FluentValidation;

namespace Application.Validators.Products
{
    public class ProductUpdateValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Geçerli bir ürün ID'si zorunludur.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün adı boş olamaz.")
                .MaximumLength(250).WithMessage("Ürün adı en fazla 250 karakter olabilir.");

            RuleFor(x => x.Gtin)
                .NotEmpty().WithMessage("GTIN alanı zorunludur.")
                .Length(14).WithMessage("GTIN 14 hane olmalıdır.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Geçerli bir müşteri seçilmelidir.");
        }
    }
}
