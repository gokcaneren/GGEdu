using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GGEdu.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EmailVerificationFieldsAddedToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EmailVerificationSentAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailVerificationToken",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerificationSentAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailVerificationToken",
                table: "Users");
        }
    }
}
