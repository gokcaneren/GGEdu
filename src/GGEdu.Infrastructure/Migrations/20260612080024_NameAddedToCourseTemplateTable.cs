using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GGEdu.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NameAddedToCourseTemplateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CourseTemplates",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "CourseTemplates");
        }
    }
}
