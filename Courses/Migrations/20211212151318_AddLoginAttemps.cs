using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Courses.Migrations
{
    public partial class AddLoginAttemps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "login_Attempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ip_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attemps = table.Column<int>(type: "int", nullable: false),
                    Blocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_login_Attempts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "login_Attempts");
        }
    }
}
