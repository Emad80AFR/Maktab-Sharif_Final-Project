using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM._Domain.ProductAgg;

namespace SM._Infrastructure.EFCore.Mapping;

public class ProductMapping:IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Code).HasMaxLength(15).IsRequired();
        builder.Property(x => x.ShortDescription).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Picture).HasMaxLength(1000);
        builder.Property(x => x.PictureAlt).HasMaxLength(255);
        builder.Property(x => x.PictureTitle).HasMaxLength(500);

        builder.Property(x => x.Keywords).HasMaxLength(100).IsRequired();
        builder.Property(x => x.MetaDescription).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Slug).HasMaxLength(500).IsRequired();

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId);

        builder.HasMany(x => x.ProductPictures)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);

        #region SeedData

        //builder.HasData(
        //    new Product
        //    {
        //        Id = 1,
        //        Name = "Product 1",
        //        Code = "P001",
        //        ShortDescription = "Product 1 Description",
        //        Picture = "product1.jpg",
        //        PictureAlt = "Product 1 Alt",
        //        PictureTitle = "Product 1 Title",
        //        Keywords = "Product 1, Keywords",
        //        MetaDescription = "Product 1 Meta Description",
        //        Slug = "product-1",
        //        CategoryId = 1
        //    },
        //    new Product
        //    {
        //        Id = 2,
        //        Name = "Product 2",
        //        Code = "P002",
        //        ShortDescription = "Product 2 Description",
        //        Picture = "product2.jpg",
        //        PictureAlt = "Product 2 Alt",
        //        PictureTitle = "Product 2 Title",
        //        Keywords = "Product 2, Keywords",
        //        MetaDescription = "Product 2 Meta Description",
        //        Slug = "product-2",
        //        CategoryId = 1
        //    },
        //    new Product
        //    {
        //        Id = 3,
        //        Name = "Product 3",
        //        Code = "P003",
        //        ShortDescription = "Product 3 Description",
        //        Picture = "product3.jpg",
        //        PictureAlt = "Product 3 Alt",
        //        PictureTitle = "Product 3 Title",
        //        Keywords = "Product 3, Keywords",
        //        MetaDescription = "Product 3 Meta Description",
        //        Slug = "product-3",
        //        CategoryId = 2
        //    }
        //);

        #endregion

    }
}