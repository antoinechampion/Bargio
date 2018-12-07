using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class ajout_RaccourcisProms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PromsKeyboardShortcut",
                columns: table => new
                {
                    Raccourci = table.Column<string>(nullable: false),
                    TBK = table.Column<string>(nullable: true),
                    Proms = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromsKeyboardShortcut", x => x.Raccourci);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromsKeyboardShortcut");
        }
    }
}
