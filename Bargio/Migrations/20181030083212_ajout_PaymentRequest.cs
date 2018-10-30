using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class ajout_PaymentRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentRequest",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Montant = table.Column<decimal>(nullable: false),
                    DateDemande = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRequest", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentRequest");
        }
    }
}
