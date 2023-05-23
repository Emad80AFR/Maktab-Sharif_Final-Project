using IM._Domain.InventoryAgg;
using IM._Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace IM._Infrastructure.EFCore
{
    public class InventoryContext:DbContext
    {
        public DbSet<Inventory> Inventory { get; set; }

        public InventoryContext(DbContextOptions<InventoryContext>options):base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryMapping).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}