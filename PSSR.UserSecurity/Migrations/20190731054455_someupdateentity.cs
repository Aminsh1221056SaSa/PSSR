using Microsoft.EntityFrameworkCore.Migrations;

namespace PSSR.UserSecurity.Migrations
{
    public partial class someupdateentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NavigationMenuItemRole_NavigationMenus_NavigationMenuItemId",
                table: "NavigationMenuItemRole");

            migrationBuilder.DropForeignKey(
                name: "FK_NavigationMenuItemRole_AspNetRoles_RoleId",
                table: "NavigationMenuItemRole");

            migrationBuilder.AddForeignKey(
                name: "FK_NavigationMenuItemRole_NavigationMenus_NavigationMenuItemId",
                table: "NavigationMenuItemRole",
                column: "NavigationMenuItemId",
                principalTable: "NavigationMenus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NavigationMenuItemRole_AspNetRoles_RoleId",
                table: "NavigationMenuItemRole",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NavigationMenuItemRole_NavigationMenus_NavigationMenuItemId",
                table: "NavigationMenuItemRole");

            migrationBuilder.DropForeignKey(
                name: "FK_NavigationMenuItemRole_AspNetRoles_RoleId",
                table: "NavigationMenuItemRole");

            migrationBuilder.AddForeignKey(
                name: "FK_NavigationMenuItemRole_NavigationMenus_NavigationMenuItemId",
                table: "NavigationMenuItemRole",
                column: "NavigationMenuItemId",
                principalTable: "NavigationMenus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NavigationMenuItemRole_AspNetRoles_RoleId",
                table: "NavigationMenuItemRole",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
