using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class update_SystemParameters_ajout_tx_lydia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CommissionLydiaFixe",
                table: "SystemParameters",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionLydiaVariable",
                table: "SystemParameters",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumRechargementLydia",
                table: "SystemParameters",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommissionLydiaFixe",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "CommissionLydiaVariable",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "MinimumRechargementLydia",
                table: "SystemParameters");
        }
    }
}
