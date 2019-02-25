using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class remove_SystemParameters_bucquagesbloques : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BucquagesBloques",
                table: "SystemParameters");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BucquagesBloques",
                table: "SystemParameters",
                nullable: false,
                defaultValue: false);
        }
    }
}
