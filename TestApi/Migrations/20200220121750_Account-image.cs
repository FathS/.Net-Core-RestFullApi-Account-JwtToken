using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestApi.Migrations
{
    public partial class Accountimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangePassTime",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "ConfirPassword",
                table: "Account");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Account",
                newName: "Image");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Account",
                newName: "Password");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangePassTime",
                table: "Account",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConfirPassword",
                table: "Account",
                nullable: true);
        }
    }
}
