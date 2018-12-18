using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class update_SystemParameters_seuil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MiseHorsBabasseSeuil",
                table: "SystemParameters",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MiseHorsBabasseSeuil",
                table: "SystemParameters");
        }
    }
}
