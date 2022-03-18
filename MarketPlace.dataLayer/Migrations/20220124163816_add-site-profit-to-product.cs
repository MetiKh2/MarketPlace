using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketPlace.dataLayer.Migrations
{
    public partial class addsiteprofittoproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteProfit",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteProfit",
                table: "Products");
        }
    }
}
