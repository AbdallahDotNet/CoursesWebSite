using Microsoft.EntityFrameworkCore.Migrations;

namespace Courses.Migrations
{
    public partial class EditDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is_Active",
                table: "Soicals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Active",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Active",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Active",
                table: "NewsLetters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Active",
                table: "login_Attempts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Active",
                table: "Instructors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Active",
                table: "Galleries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Active",
                table: "Faqs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Active",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Active",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Active",
                table: "Course_specialties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Active",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Active",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Active",
                table: "Blogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_Active",
                table: "Application_intros",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_Active",
                table: "Soicals");

            migrationBuilder.DropColumn(
                name: "Is_Active",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Is_Active",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Is_Active",
                table: "NewsLetters");

            migrationBuilder.DropColumn(
                name: "Is_Active",
                table: "login_Attempts");

            migrationBuilder.DropColumn(
                name: "Is_Active",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "Is_Active",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "Is_Active",
                table: "Faqs");

            migrationBuilder.DropColumn(
                name: "Is_Active",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Is_Active",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Is_Active",
                table: "Course_specialties");

            migrationBuilder.DropColumn(
                name: "Is_Active",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Is_Active",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Is_Active",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Is_Active",
                table: "Application_intros");
        }
    }
}
