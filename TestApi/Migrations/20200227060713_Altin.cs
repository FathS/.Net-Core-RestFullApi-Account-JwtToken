using Microsoft.EntityFrameworkCore.Migrations;

namespace TestApi.Migrations
{
    public partial class Altin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AltinKur",
                table: "Balance",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BuyAltin",
                table: "Balance",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SellAltin",
                table: "Balance",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AltinKur",
                table: "Balance");

            migrationBuilder.DropColumn(
                name: "BuyAltin",
                table: "Balance");

            migrationBuilder.DropColumn(
                name: "SellAltin",
                table: "Balance");
        }
    }
}
