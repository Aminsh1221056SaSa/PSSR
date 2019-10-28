using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PSSR.UserSecurity.Migrations
{
    public partial class addclientNametoMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NavigationMenuItemRole_NavigationMenus_NavigationMenuItemId",
                table: "NavigationMenuItemRole");

            migrationBuilder.DropTable(
                name: "NavigationMenus");

            migrationBuilder.EnsureSchema(
                name: "Setting");

            migrationBuilder.RenameTable(
                name: "NavigationMenuItemRole",
                newName: "NavigationMenuItemRole",
                newSchema: "Setting");

            migrationBuilder.CreateTable(
                name: "NavigationMenuType",
                schema: "Setting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DisplayName = table.Column<string>(maxLength: 150, nullable: false),
                    MaterialIcon = table.Column<string>(maxLength: 150, nullable: true),
                    Link = table.Column<string>(maxLength: 500, nullable: true),
                    IsNested = table.Column<bool>(nullable: false),
                    Sequence = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    ClientName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavigationMenuType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NavigationMenuType_NavigationMenuType_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Setting",
                        principalTable: "NavigationMenuType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NavigationMenuType_ParentId",
                schema: "Setting",
                table: "NavigationMenuType",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_NavigationMenuItemRole_NavigationMenuType_NavigationMenuItemId",
                schema: "Setting",
                table: "NavigationMenuItemRole",
                column: "NavigationMenuItemId",
                principalSchema: "Setting",
                principalTable: "NavigationMenuType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NavigationMenuItemRole_NavigationMenuType_NavigationMenuItemId",
                schema: "Setting",
                table: "NavigationMenuItemRole");

            migrationBuilder.DropTable(
                name: "NavigationMenuType",
                schema: "Setting");

            migrationBuilder.RenameTable(
                name: "NavigationMenuItemRole",
                schema: "Setting",
                newName: "NavigationMenuItemRole");

            migrationBuilder.CreateTable(
                name: "NavigationMenus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DisplayName = table.Column<string>(maxLength: 150, nullable: false),
                    IsNested = table.Column<bool>(nullable: false),
                    Link = table.Column<string>(maxLength: 500, nullable: true),
                    MaterialIcon = table.Column<string>(maxLength: 150, nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    Sequence = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_NavigationMenus_ParentId",
                table: "NavigationMenus",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_NavigationMenuItemRole_NavigationMenus_NavigationMenuItemId",
                table: "NavigationMenuItemRole",
                column: "NavigationMenuItemId",
                principalTable: "NavigationMenus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
