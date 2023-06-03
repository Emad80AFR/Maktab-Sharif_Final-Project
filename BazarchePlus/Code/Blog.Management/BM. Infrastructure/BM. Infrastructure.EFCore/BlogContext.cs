using BM._Domain.ArticleAgg;
using BM._Domain.ArticleCategoryAgg;
using BM._Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace BM._Infrastructure.EFCore
{
    public class BlogContext:DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public BlogContext(DbContextOptions<BlogContext>options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArticleCategoryMapping).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}