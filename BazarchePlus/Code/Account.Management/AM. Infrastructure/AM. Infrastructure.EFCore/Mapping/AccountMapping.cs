using AM._Domain.AccountAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM._Infrastructure.EFCore.Mapping;

public class AccountMapping:IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Fullname).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Username).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Password).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.ProfilePhoto).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Mobile).HasMaxLength(20).IsRequired();
        builder.Property(x => x.ShopName).HasMaxLength(100);
        builder.Property(x => x.ShopPhoto).HasMaxLength(500);

        builder.HasOne(x => x.Role).WithMany(x => x.Accounts).HasForeignKey(x => x.RoleId);

        #region MyRegion

        //builder.HasData(
        //    new Account
        //    {
        //        Id = 1,
        //        Fullname = "John Doe",
        //        Username = "johndoe",
        //        Password = "password1",
        //        Mobile = "1234567890",
        //        RoleId = 1,
        //        ProfilePhoto = "profile1.jpg"
        //    },
        //    new Account
        //    {
        //        Id = 2,
        //        Fullname = "Jane Smith",
        //        Username = "janesmith",
        //        Password = "password2",
        //        Mobile = "0987654321",
        //        RoleId = 2,
        //        ProfilePhoto = "profile2.jpg"
        //    }
        //);

        #endregion
    }
}