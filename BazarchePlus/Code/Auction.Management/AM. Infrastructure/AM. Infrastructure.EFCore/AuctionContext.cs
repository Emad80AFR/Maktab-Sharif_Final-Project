using AM._Domain.AuctionAgg;
using AM._Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AM._Infrastructure.EFCore
{
    public class AuctionContext:DbContext
    {
        public DbSet<Auction> Auctions { get; set; }

        public AuctionContext(DbContextOptions<AuctionContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuctionMapping).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}