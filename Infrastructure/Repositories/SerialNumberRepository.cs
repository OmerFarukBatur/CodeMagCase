using Core.Entities;
using Core.Interfaces.IRepositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class SerialNumberRepository : Repository<SerialNumber>, ISerialNumberRepository
    {
        public SerialNumberRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
