using Microsoft.EntityFrameworkCore.Migrations;

namespace Courses.Migrations
{
    public partial class RemoveGoogle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Google_plus",
                table: "Soicals");

            migrationBuilder.DropColumn(
                name: "Google_plus",
                table: "Instructors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Google_plus",
                table: "Soicals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Google_plus",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
