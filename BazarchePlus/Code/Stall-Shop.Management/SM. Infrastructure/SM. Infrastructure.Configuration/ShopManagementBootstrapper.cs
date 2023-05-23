using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SM._Application.Contracts.ProductCategory;
using SM._Application.Implementation;
using SM._Domain.ProductCategoryAgg;
using SM._Infrastructure.EFCore;
using SM._Infrastructure.EFCore.Repository;

namespace SM._Infrastructure.Configuration
{
    public class ShopManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ShopContext>(x => x.UseSqlServer(connectionString));


            services.AddScoped<IProductCategoryApplication, ProductCategoryApplication>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        }
    }
}