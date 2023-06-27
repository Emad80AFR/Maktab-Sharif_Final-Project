using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CM._Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddSender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Sender",
                table: "Comments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sender",
                table: "Comments");
        }
    }
}
