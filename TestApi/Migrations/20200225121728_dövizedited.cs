using Microsoft.EntityFrameworkCore.Migrations;

namespace TestApi.Migrations
{
    public partial class dövizedited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BuyTL",
                table: "Balance",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SellEURO",
                table: "Balance",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SellUSD",
                table: "Balance",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyTL",
                table: "Balance");

            migrationBuilder.DropColumn(
                name: "SellEURO",
                table: "Balance");

            migrationBuilder.DropColumn(
                name: "SellUSD",
                table: "Balance");
        }
    }
}
