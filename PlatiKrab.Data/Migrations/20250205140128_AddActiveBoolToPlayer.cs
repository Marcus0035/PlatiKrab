using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatiKrab.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveBoolToPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Players");
        }
    }
}
