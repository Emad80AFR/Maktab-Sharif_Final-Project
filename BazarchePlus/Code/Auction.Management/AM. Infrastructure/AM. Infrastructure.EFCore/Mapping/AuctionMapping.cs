using AM._Domain.AuctionAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM._Infrastructure.EFCore.Mapping;

public class AuctionMapping:IEntityTypeConfiguration<Auction>
{
    public void Configure(EntityTypeBuilder<Auction> builder)
    {
        builder.ToTable("Auctions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.WinnerUsername).HasMaxLength(250);

    }
}