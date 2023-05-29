using DM._Domain.ColleagueDiscountAgg;
using DM._Domain.CustomerDiscountAgg;
using DM._Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace DM._Infrastructure.EFCore
{
    public class DiscountContext:DbContext
    {
        public DbSet<CustomerDiscount> CustomerDiscounts { get; set; }
        public DbSet<ColleagueDiscount> ColleagueDiscounts { get; set; }
        public DiscountContext(DbContextOptions<DiscountContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDiscountMapping).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}