using Microsoft.EntityFrameworkCore.Migrations;

namespace Courses.Migrations
{
    public partial class EditIsActiveName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Soicals",
                newName: "Is_active");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Settings",
                newName: "Is_active");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Notifications",
                newName: "Is_active");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "NewsLetters",
                newName: "Is_active");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "login_Attempts",
                newName: "Is_active");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Instructors",
                newName: "Is_active");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Galleries",
                newName: "Is_active");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Faqs",
                newName: "Is_active");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Events",
                newName: "Is_active");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Courses",
                newName: "Is_active");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Course_specialties",
                newName: "Is_active");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Comments",
                newName: "Is_active");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Bookings",
                newName: "Is_active");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Blogs",
                newName: "Is_active");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Application_intros",
                newName: "Is_active");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Is_active",
                table: "Soicals",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "Is_active",
                table: "Settings",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "Is_active",
                table: "Notifications",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "Is_active",
                table: "NewsLetters",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "Is_active",
                table: "login_Attempts",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "Is_active",
                table: "Instructors",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "Is_active",
                table: "Galleries",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "Is_active",
                table: "Faqs",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "Is_active",
                table: "Events",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "Is_active",
                table: "Courses",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "Is_active",
                table: "Course_specialties",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "Is_active",
                table: "Comments",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "Is_active",
                table: "Bookings",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "Is_active",
                table: "Blogs",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "Is_active",
                table: "Application_intros",
                newName: "Is_Active");
        }
    }
}
