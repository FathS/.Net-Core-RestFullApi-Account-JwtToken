using Microsoft.EntityFrameworkCore.Migrations;

namespace TestApi.Migrations
{
    public partial class aaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DovizKur",
                table: "Balance",
                newName: "EuroKur");

            migrationBuilder.AddColumn<decimal>(
                name: "DolarKur",
                table: "Balance",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DolarKur",
                table: "Balance");

            migrationBuilder.RenameColumn(
                name: "EuroKur",
                table: "Balance",
                newName: "DovizKur");
        }
    }
}
