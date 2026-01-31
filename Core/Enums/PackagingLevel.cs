using System.ComponentModel.DataAnnotations;

namespace Core.Enums
{
    public enum PackagingLevel
    {
        [Display(Name = "Koli")]
        Case = 1,
        [Display(Name = "Palet")]
        Pallet = 2 
    }
}
