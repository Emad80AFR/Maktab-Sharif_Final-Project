using BM._Application.Contracts.Article;
using BM._Application.Contracts.ArticleCategory;
using BM._Application.Implementation;
using BM._Domain.ArticleAgg;
using BM._Domain.ArticleCategoryAgg;
using BM._Infrastructure.EFCore;
using BM._Infrastructure.EFCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BM._Infrastructure.Configuration
{
    public class BlogManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<BlogContext>(x => x.UseSqlServer(connectionString));

            services.AddScoped<IArticleCategoryApplication, ArticleCategoryApplication>();
            services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();

            services.AddScoped<IArticleApplication, ArticleApplication>();
            services.AddScoped<IArticleRepository, ArticleRepository>();

        }
    }
}