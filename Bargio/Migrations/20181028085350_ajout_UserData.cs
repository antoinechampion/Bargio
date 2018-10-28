using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class ajout_UserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserData",
                columns: table => new
                {
                    UserName = table.Column<string>(nullable: false),
                    Solde = table.Column<decimal>(nullable: false),
                    Nums = table.Column<long>(nullable: false),
                    TBK = table.Column<string>(nullable: true),
                    Proms = table.Column<long>(nullable: false),
                    HorsFoys = table.Column<bool>(nullable: false),
                    Surnom = table.Column<string>(nullable: true),
                    DateDerniereModif = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.UserName);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserData");
        }
    }
}
