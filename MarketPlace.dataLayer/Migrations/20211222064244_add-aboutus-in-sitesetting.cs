using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketPlace.dataLayer.Migrations
{
    public partial class addaboutusinsitesetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboutUS",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutUS",
                table: "SiteSettings");
        }
    }
}
