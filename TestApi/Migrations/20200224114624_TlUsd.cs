using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestApi.Migrations
{
    public partial class TlUsd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "Account");

            migrationBuilder.AddColumn<decimal>(
                name: "TL",
                table: "Account",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "USD",
                table: "Account",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TL",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "USD",
                table: "Account");

            migrationBuilder.AddColumn<Guid>(
                name: "InventoryId",
                table: "Account",
                nullable: true);
        }
    }
}
