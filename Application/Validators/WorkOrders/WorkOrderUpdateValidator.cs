using Core.DTOs.WorkOrders;
using FluentValidation;

namespace Application.Validators.WorkOrders
{
    public class WorkOrderUpdateValidator : AbstractValidator<WorkOrderUpdateDto>
    {
        public WorkOrderUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Geçerli bir ürün seçilmelidir.");
            RuleFor(x => x.TargetQuantity).InclusiveBetween(1, 10000).WithMessage("Üretim adedi 1 ile 10.000 arasında olmalıdır.");
            RuleFor(x => x.LotNumber).NotEmpty().MaximumLength(20).WithMessage("Lot numarası zorunludur.");
            RuleFor(x => x.ExpiryDate).GreaterThan(DateTime.Now).WithMessage("Son kullanma tarihi bugünden büyük olmalıdır.");
            RuleFor(x => x.WorkOrderStatusId).InclusiveBetween(1, 4).WithMessage("Bir iş durumunu seçiniz.");
        }
    }
}
