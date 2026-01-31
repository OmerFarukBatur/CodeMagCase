using Core.Entities;
using Core.Interfaces.IRepositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class PackagingUnitRepository : Repository<PackagingUnit>, IPackagingUnitRepository
    {
        public PackagingUnitRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
