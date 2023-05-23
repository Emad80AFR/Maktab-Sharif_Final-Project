using Microsoft.EntityFrameworkCore;
using SM._Domain.ProductAgg;
using SM._Domain.ProductCategoryAgg;
using SM._Infrastructure.EFCore.Mapping;

namespace SM._Infrastructure.EFCore
{
    public class ShopContext:DbContext
    {
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public ShopContext(DbContextOptions<ShopContext> options):base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductCategoryMapping).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}