using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM._Domain.ProductPictureAgg;

namespace SM._Infrastructure.EFCore.Mapping;

public class ProductPictureMapping:IEntityTypeConfiguration<ProductPicture>
{
    public void Configure(EntityTypeBuilder<ProductPicture> builder)
    {
        builder.ToTable("ProductPictures");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Picture).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.PictureAlt).HasMaxLength(500).IsRequired();
        builder.Property(x => x.PictureTitle).HasMaxLength(500).IsRequired();

        builder.HasOne(x => x.Product)
            .WithMany(x => x.ProductPictures)
            .HasForeignKey(x => x.ProductId);

        #region SeedData

        //builder.HasData(
        //    new ProductPicture
        //    {
        //        Id = 1,
        //        Picture = "product1-1.jpg",
        //        PictureAlt = "Product 1 Picture 1 Alt",
        //        PictureTitle = "Product 1 Picture 1 Title",
        //        CreationDate = DateTime.Now,
        //        ProductId = 1
        //    },
        //    new ProductPicture
        //    {
        //        Id = 2,
        //        Picture = "product1-2.jpg",
        //        PictureAlt = "Product 1 Picture 2 Alt",
        //        PictureTitle = "Product 1 Picture 2 Title",
        //        CreationDate = DateTime.Now,
        //        ProductId = 1
        //    },
        //    new ProductPicture
        //    {
        //        Id = 3,
        //        Picture = "product2-1.jpg",
        //        PictureAlt = "Product 2 Picture 1 Alt",
        //        PictureTitle = "Product 2 Picture 1 Title",
        //        CreationDate = DateTime.Now,
        //        ProductId = 2
        //    }
        //);

        #endregion
    }
}