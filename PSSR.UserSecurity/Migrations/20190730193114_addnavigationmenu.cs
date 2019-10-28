using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PSSR.UserSecurity.Migrations
{
    public partial class addnavigationmenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NavigationMenus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DisplayName = table.Column<string>(maxLength: 150, nullable: false),
                    MaterialIcon = table.Column<string>(maxLength: 150, nullable: true),
                    Link = table.Column<string>(maxLength: 500, nullable: true),
                    IsNested = table.Column<bool>(nullable: false),
                    Sequence = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavigationMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NavigationMenus_NavigationMenus_ParentId",
                        column: x => x.ParentId,
                        principalTable: "NavigationMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NavigationMenuItemRole",
                columns: table => new
                {
                    NavigationMenuItemId = table.Column<int>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavigationMenuItemRole", x => new { x.NavigationMenuItemId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_NavigationMenuItemRole_NavigationMenus_NavigationMenuItemId",
                        column: x => x.NavigationMenuItemId,
                        principalTable: "NavigationMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NavigationMenuItemRole_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NavigationMenuItemRole_RoleId",
                table: "NavigationMenuItemRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_NavigationMenus_ParentId",
                table: "NavigationMenus",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NavigationMenuItemRole");

            migrationBuilder.DropTable(
                name: "NavigationMenus");
        }
    }
}
