using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HKDR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addnewtablesforhr1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "PayrollTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "PayrollTransactions");
        }
    }
}
