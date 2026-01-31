using Application.Services;
using Application.Validators.Customers;
using Application.Validators.Packagings;
using Application.Validators.Products;
using Application.Validators.WorkOrders;
using Core.Interfaces.IServices;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            // Servisler
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IWorkOrderService, WorkOrderService>();
            services.AddScoped<IPackagingService, PackagingService>();

            // FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //services.AddValidatorsFromAssemblyContaining<CustomerCreateValidator>();
            //services.AddValidatorsFromAssemblyContaining<CustomerUpdateValidator>();
            //services.AddValidatorsFromAssemblyContaining<ProductCreateValidator>();
            //services.AddValidatorsFromAssemblyContaining<ProductUpdateValidator>();
            //services.AddValidatorsFromAssemblyContaining<WorkOrderCreateValidator>();
            //services.AddValidatorsFromAssemblyContaining<WorkOrderUpdateValidator>();
            //services.AddValidatorsFromAssemblyContaining<PackagingCreateValidator>();
            //services.AddValidatorsFromAssemblyContaining<PackagingUpdateValidator>();
        }
    }
}
