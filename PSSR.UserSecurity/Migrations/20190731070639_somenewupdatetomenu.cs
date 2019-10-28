using Microsoft.EntityFrameworkCore.Migrations;

namespace PSSR.UserSecurity.Migrations
{
    public partial class somenewupdatetomenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "NavigationMenus",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "NavigationMenus");
        }
    }
}
