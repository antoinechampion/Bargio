using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class update_SystemParameters_settings_zifoys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdProduits",
                table: "TransactionHistory",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MiseHorsBabasseAutoActivee",
                table: "SystemParameters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MiseHorsBabasseHebdomadaireHeure",
                table: "SystemParameters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiseHorsBabasseHebdomadaireJours",
                table: "SystemParameters",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MiseHorsBabasseInstantanee",
                table: "SystemParameters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MiseHorsBabasseQuotidienne",
                table: "SystemParameters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MiseHorsBabasseQuotidienneHeure",
                table: "SystemParameters",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MiseHorsBabasseAutoActivee",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "MiseHorsBabasseHebdomadaireHeure",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "MiseHorsBabasseHebdomadaireJours",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "MiseHorsBabasseInstantanee",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "MiseHorsBabasseQuotidienne",
                table: "SystemParameters");

            migrationBuilder.DropColumn(
                name: "MiseHorsBabasseQuotidienneHeure",
                table: "SystemParameters");

            migrationBuilder.AlterColumn<long>(
                name: "IdProduits",
                table: "TransactionHistory",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
