using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class ajout_salt_UserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FoysApiPasswordSalt",
                table: "UserData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoysApiPasswordSalt",
                table: "UserData");
        }
    }
}
