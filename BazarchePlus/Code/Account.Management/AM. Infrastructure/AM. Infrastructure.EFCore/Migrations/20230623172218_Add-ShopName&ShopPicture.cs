using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AM._Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddShopNameShopPicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShopName",
                table: "Accounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShopPhoto",
                table: "Accounts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShopName",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ShopPhoto",
                table: "Accounts");
        }
    }
}
