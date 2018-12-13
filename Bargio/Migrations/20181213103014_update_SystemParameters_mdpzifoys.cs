using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class update_SystemParameters_mdpzifoys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MotDePasseZifoys",
                table: "SystemParameters",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotDePasseZifoys",
                table: "SystemParameters");
        }
    }
}
