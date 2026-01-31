using System.ComponentModel.DataAnnotations;

namespace Core.Enums
{
    public enum WorkOrderStatus
    {
        [Display(Name = "İş emri oluşturuldu")]
        Draft = 1,
        [Display(Name = "Üretim hattında, seri no üretiliyor")]
        Active = 2,   
        [Display(Name = "Hedef adete ulaşıldı")]
        Completed = 3, 
        [Display(Name = "İş iptal edildi")]
        Cancelled = 4 
    }
}