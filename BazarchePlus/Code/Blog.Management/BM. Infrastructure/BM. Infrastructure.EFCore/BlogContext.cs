using BM._Domain.ArticleCategoryAgg;
using Microsoft.EntityFrameworkCore;

namespace BM._Infrastructure.EFCore
{
    public class BlogContext:DbContext
    {
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public BlogContext(DbContextOptions<BlogContext>options):base(options)
        {
            
        }
    }
}