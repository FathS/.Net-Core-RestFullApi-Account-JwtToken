using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestApi.Migrations
{
    public partial class uspasschanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangedPassword",
                table: "UserPassword");

            migrationBuilder.AddColumn<bool>(
                name: "ActivePassword",
                table: "UserPassword",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivePassword",
                table: "UserPassword");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedPassword",
                table: "UserPassword",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
