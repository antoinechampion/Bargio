using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class update_SystemeParameters_snow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Actualites",
                table: "SystemParameters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotDesZifoys",
                table: "SystemParameters",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Snow",
                table: "SystemParameters",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actualites",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "MotDesZifoys",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "Snow",
                table: "SystemParameters");
        }
    }
}
