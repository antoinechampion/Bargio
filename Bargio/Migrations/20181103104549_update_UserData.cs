using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class update_UserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "UserData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prenom",
                table: "UserData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telephone",
                table: "UserData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nom",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "Prenom",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "Telephone",
                table: "UserData");
        }
    }
}
