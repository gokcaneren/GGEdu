using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GGEdu.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewCourseTemplateBookingSystemUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_AvailabilityCourseSlotId",
                table: "Bookings");

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherCourseId",
                table: "CourseTemplates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DecisionDate",
                table: "Bookings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherCourseId",
                table: "AvailabilityCourseSlots",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CourseTemplates_TeacherCourseId",
                table: "CourseTemplates",
                column: "TeacherCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Code",
                table: "Courses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AvailabilityCourseSlotId",
                table: "Bookings",
                column: "AvailabilityCourseSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityCourseSlots_TeacherCourseId",
                table: "AvailabilityCourseSlots",
                column: "TeacherCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AvailabilityCourseSlots_TeacherCourses_TeacherCourseId",
                table: "AvailabilityCourseSlots",
                column: "TeacherCourseId",
                principalTable: "TeacherCourses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTemplates_TeacherCourses_TeacherCourseId",
                table: "CourseTemplates",
                column: "TeacherCourseId",
                principalTable: "TeacherCourses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvailabilityCourseSlots_TeacherCourses_TeacherCourseId",
                table: "AvailabilityCourseSlots");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseTemplates_TeacherCourses_TeacherCourseId",
                table: "CourseTemplates");

            migrationBuilder.DropIndex(
                name: "IX_CourseTemplates_TeacherCourseId",
                table: "CourseTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Courses_Code",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_AvailabilityCourseSlotId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_AvailabilityCourseSlots_TeacherCourseId",
                table: "AvailabilityCourseSlots");

            migrationBuilder.DropColumn(
                name: "TeacherCourseId",
                table: "CourseTemplates");

            migrationBuilder.DropColumn(
                name: "DecisionDate",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TeacherCourseId",
                table: "AvailabilityCourseSlots");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AvailabilityCourseSlotId",
                table: "Bookings",
                column: "AvailabilityCourseSlotId",
                unique: true);
        }
    }
}
