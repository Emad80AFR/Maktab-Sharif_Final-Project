using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AM._Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerIdToAuction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SellerId",
                table: "Auctions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Auctions");
        }
    }
}
