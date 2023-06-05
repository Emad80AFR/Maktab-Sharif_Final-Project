using CM._Domain.CommentAgg;
using CM._Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CM._Infrastructure.EFCore
{
    public class CommentContext:DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public CommentContext(DbContextOptions<CommentContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CommentMapping).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}