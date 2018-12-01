using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class ajout_hash_UserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FoysApiHasPassword",
                table: "UserData",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FoysApiPasswordHash",
                table: "UserData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoysApiHasPassword",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "FoysApiPasswordHash",
                table: "UserData");
        }
    }
}
