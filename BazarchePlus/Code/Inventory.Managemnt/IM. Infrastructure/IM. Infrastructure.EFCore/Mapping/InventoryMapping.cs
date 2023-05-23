using IM._Domain.InventoryAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM._Infrastructure.EFCore.Mapping;

public class InventoryMapping:IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable("Inventory");
        builder.HasKey(x => x.Id);

        builder.OwnsMany(x => x.Operations, modelBuilder =>
        {
            modelBuilder.HasKey(x => x.Id);
            modelBuilder.ToTable("InventoryOperations");
            modelBuilder.Property(x => x.Description).HasMaxLength(1000);
            modelBuilder.WithOwner(x => x.Inventory).HasForeignKey(x => x.InventoryId);
        });

        #region SeedData

        //builder.HasData(
        //    new Inventory
        //    {
        //        Id = 1,
        //        ProductId = 1,
        //        UnitPrice = 10.99,
        //        InStock = true
        //    },
        //    new Inventory
        //    {
        //        Id = 2,
        //        ProductId = 2,
        //        UnitPrice = 15.99,
        //        InStock = true
        //    }
        //);

        #endregion
    }
}