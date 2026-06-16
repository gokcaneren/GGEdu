using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GGEdu.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TeacherTableRenewed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Teachers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "Teachers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Teachers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
