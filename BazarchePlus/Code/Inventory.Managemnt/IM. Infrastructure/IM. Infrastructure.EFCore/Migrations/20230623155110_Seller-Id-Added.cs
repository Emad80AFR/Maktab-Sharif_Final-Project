using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IM._Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class SellerIdAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SellerId",
                table: "Inventory",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Inventory");
        }
    }
}
