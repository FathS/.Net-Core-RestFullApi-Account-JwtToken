using Microsoft.EntityFrameworkCore.Migrations;

namespace TestApi.Migrations
{
    public partial class cokk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AltinGr",
                table: "Account",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AltinGr",
                table: "Account");
        }
    }
}
