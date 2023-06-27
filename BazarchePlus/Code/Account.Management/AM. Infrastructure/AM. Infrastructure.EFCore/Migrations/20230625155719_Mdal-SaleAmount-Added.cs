using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AM._Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class MdalSaleAmountAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Medal",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "SalesAmount",
                table: "Accounts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Medal",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "SalesAmount",
                table: "Accounts");
        }
    }
}
