using Core.Entities.Common;
using Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class PackagingUnit : BaseEntity
    {
        public string Sscc { get; set; }

        public int PackagingLevelId { get; set; }

        [NotMapped]
        public PackagingLevel PackagingLevel
        {
            get => (PackagingLevel)PackagingLevelId;
            set => PackagingLevelId = (int)value;
        }

        public int? ParentId { get; set; }
        public PackagingUnit Parent { get; set; }

        public ICollection<PackagingUnit> Children { get; set; }
        public ICollection<SerialNumber> SerialNumbers { get; set; }
    }
}
