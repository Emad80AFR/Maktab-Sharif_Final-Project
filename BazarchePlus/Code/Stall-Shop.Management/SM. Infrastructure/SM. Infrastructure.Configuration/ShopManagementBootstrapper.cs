using BP._Query.Contracts.Product;
using BP._Query.Contracts.ProductCategory;
using BP._Query.Contracts.Slide;
using BP._Query.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SM._Application.Contracts.Product;
using SM._Application.Contracts.ProductCategory;
using SM._Application.Contracts.ProductPicture;
using SM._Application.Contracts.Slide;
using SM._Application.Implementation;
using SM._Domain.ProductAgg;
using SM._Domain.ProductCategoryAgg;
using SM._Domain.ProductPictureAgg;
using SM._Domain.SlideAgg;
using SM._Infrastructure.EFCore;
using SM._Infrastructure.EFCore.Repository;

namespace SM._Infrastructure.Configuration
{
    public class ShopManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<ShopContext>(x => x.UseSqlServer(connectionString));


            services.AddScoped<IProductCategoryApplication, ProductCategoryApplication>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IProductCategoryQuery,ProductCategoryQuery>();

            services.AddScoped<IProductApplication, ProductApplication>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductQuery, ProductQuery>();

            services.AddScoped<IProductPictureApplication, ProductPictureApplication>();
            services.AddScoped<IProductPictureRepository, ProductPictureRepository>();

            services.AddScoped<ISlideApplication, SlideApplication>();
            services.AddScoped<ISlideRepository, SlideRepository>();
            services.AddScoped<ISlideQuery, SlideQuery>();
        }
    }
}