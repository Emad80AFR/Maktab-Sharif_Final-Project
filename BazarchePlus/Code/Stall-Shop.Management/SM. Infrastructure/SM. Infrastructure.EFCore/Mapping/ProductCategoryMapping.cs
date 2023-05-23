using System.Drawing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM._Domain.ProductCategoryAgg;

namespace SM._Infrastructure.EFCore.Mapping
{
    public class ProductCategoryMapping:IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.ToTable("ProductCategories");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Picture).HasMaxLength(1000);
            builder.Property(x => x.PictureAlt).HasMaxLength(255);
            builder.Property(x => x.PictureTitle).HasMaxLength(500);
            builder.Property(x => x.Keywords).HasMaxLength(80).IsRequired();
            builder.Property(x => x.MetaDescription).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Slug).HasMaxLength(300).IsRequired();

            builder.HasMany(x => x.Products)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId);

            #region SeedDate

        //    builder.HasData(
        //    new ProductCategory
        //    {
        //        Id = 1,
        //        Name = "Category 1",
        //        Description = "Category 1 Description",
        //        Picture = "category1.jpg",
        //        PictureAlt = "Category 1 Alt",
        //        PictureTitle = "Category 1 Title",
        //        Keywords = "Category 1, Keywords",
        //        MetaDescription = "Category 1 Meta Description",
        //        Slug = "category-1"
        //    },
        //    new ProductCategory
        //    {
        //        Id = 2,
        //        Name = "Category 2",
        //        Description = "Category 2 Description",
        //        Picture = "category2.jpg",
        //        PictureAlt = "Category 2 Alt",
        //        PictureTitle = "Category 2 Title",
        //        Keywords = "Category 2, Keywords",
        //        MetaDescription = "Category 2 Meta Description",
        //        Slug = "category-2"
        //    }
        //);

            #endregion

        }
    }
}
