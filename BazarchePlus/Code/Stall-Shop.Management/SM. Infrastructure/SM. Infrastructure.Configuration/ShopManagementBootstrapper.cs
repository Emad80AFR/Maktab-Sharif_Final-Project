using BP._Query.Contracts.Order;
using BP._Query.Contracts.Product;
using BP._Query.Contracts.ProductCategory;
using BP._Query.Contracts.Slide;
using BP._Query.Query;
using FrameWork.Infrastructure.Permission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SM._Application.Contracts.Order;
using SM._Application.Contracts.Product;
using SM._Application.Contracts.ProductCategory;
using SM._Application.Contracts.ProductPicture;
using SM._Application.Contracts.Slide;
using SM._Application.Implementation;
using SM._Domain.OrderAgg;
using SM._Domain.ProductAgg;
using SM._Domain.ProductCategoryAgg;
using SM._Domain.ProductPictureAgg;
using SM._Domain.Services;
using SM._Domain.SlideAgg;
using SM._Infrastructure.Configuration.Permissions;
using SM._Infrastructure.EFCore;
using SM._Infrastructure.EFCore.Repository;
using SM._Infrastructure.InventoryAcl;

namespace SM._Infrastructure.Configuration
{
    public class ShopManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<ShopContext>(x => x.UseSqlServer(connectionString));


            services.AddScoped<IProductCategoryQuery,ProductCategoryQuery>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IProductCategoryApplication, ProductCategoryApplication>();

            services.AddScoped<IProductQuery, ProductQuery>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductApplication, ProductApplication>();

            services.AddScoped<IProductPictureRepository, ProductPictureRepository>();
            services.AddScoped<IProductPictureApplication, ProductPictureApplication>();

            services.AddScoped<ISlideQuery, SlideQuery>();
            services.AddScoped<ISlideRepository, SlideRepository>();
            services.AddScoped<ISlideApplication, SlideApplication>();

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderApplication, OrderApplication>();

            services.AddScoped<ICartCalculatorService, CartCalculatorService>();

            services.AddSingleton<ICartService, CartService>();

            services.AddScoped<IShopInventoryAcl, ShopInventoryAcl>();

            services.AddTransient<IPermissionExposer, ShopPermissionExposer>();

        }
    }
}