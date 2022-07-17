using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Courses.Migrations
{
    public partial class EditNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NEWID()");
        }
    }
}
