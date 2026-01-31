using Core.Interfaces.IRepositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            // DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Repository'ler
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IPackagingUnitRepository, PackagingUnitRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISerialNumberRepository, SerialNumberRepository>();
            services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
        }
    }
}
