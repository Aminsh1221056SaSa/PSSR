using Microsoft.EntityFrameworkCore.Migrations;

namespace PSSR.UserSecurity.Migrations
{
    public partial class addsomeupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "NavigationMenus",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "NavigationMenus",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
