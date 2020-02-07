using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestApi.Migrations
{
    public partial class AccountChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ChangePassTime",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Account",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangePassTime",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Account");
        }
    }
}
