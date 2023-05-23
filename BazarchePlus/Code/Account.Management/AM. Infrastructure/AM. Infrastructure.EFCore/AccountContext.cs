using AM._Domain.AccountAgg;
using AM._Domain.RollAgg;
using AM._Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AM._Infrastructure.EFCore
{
    public class AccountContext:DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public AccountContext(DbContextOptions<AccountContext> options):base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountMapping).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}