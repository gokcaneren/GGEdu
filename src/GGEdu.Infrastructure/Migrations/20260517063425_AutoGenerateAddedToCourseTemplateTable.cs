using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GGEdu.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AutoGenerateAddedToCourseTemplateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TimeZoneId",
                table: "CourseTemplates",
                type: "text",
                nullable: false,
                defaultValue: "UTC",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "AutoGenerateSlots",
                table: "CourseTemplates",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "GenerateDaysAhead",
                table: "CourseTemplates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CourseTemplates",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoGenerateSlots",
                table: "CourseTemplates");

            migrationBuilder.DropColumn(
                name: "GenerateDaysAhead",
                table: "CourseTemplates");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CourseTemplates");

            migrationBuilder.AlterColumn<string>(
                name: "TimeZoneId",
                table: "CourseTemplates",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "UTC");
        }
    }
}
