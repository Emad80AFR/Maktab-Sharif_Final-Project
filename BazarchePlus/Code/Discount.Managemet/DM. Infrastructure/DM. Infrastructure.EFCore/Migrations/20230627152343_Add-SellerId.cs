using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DM._Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SellerId",
                table: "CustomerDiscounts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "CustomerDiscounts");
        }
    }
}
