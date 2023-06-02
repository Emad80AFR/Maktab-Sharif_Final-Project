using AM._Domain.RollAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM._Infrastructure.EFCore.Mapping;

public class RoleMapping:IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

        //builder.OwnsMany(x => x.Permissions, navigationBuilder =>
        //{
        //    navigationBuilder.HasKey(x => x.Id);
        //    navigationBuilder.ToTable("RolePermissions");
        //    navigationBuilder.Ignore(x => x.Name);
        //    navigationBuilder.WithOwner(x => x.Role);
        //});

        #region SeedData

        //builder.HasData(
        //    new Role
        //    {
        //        Id = 1,
        //        Name = "Admin",
        //        Permissions = new List<Permission>
        //        {
        //            new Permission { Id = 1, Name = "Permission 1" },
        //            new Permission { Id = 2, Name = "Permission 2" }
        //        }
        //    },
        //    new Role
        //    {
        //        Id = 2,
        //        Name = "User",
        //        Permissions = new List<Permission>
        //        {
        //            new Permission { Id = 3, Name = "Permission 3" }
        //        }
        //    }
        //);
        #endregion
    }



}
