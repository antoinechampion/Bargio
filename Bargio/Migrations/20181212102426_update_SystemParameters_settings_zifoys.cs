//          Bargio - 20181212102426_update_SystemParameters_settings_zifoys.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class update_SystemParameters_settings_zifoys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AlterColumn<string>(
                "IdProduits",
                "TransactionHistory",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                "MiseHorsBabasseAutoActivee",
                "SystemParameters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                "MiseHorsBabasseHebdomadaireHeure",
                "SystemParameters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "MiseHorsBabasseHebdomadaireJours",
                "SystemParameters",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                "MiseHorsBabasseInstantanee",
                "SystemParameters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                "MiseHorsBabasseQuotidienne",
                "SystemParameters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                "MiseHorsBabasseQuotidienneHeure",
                "SystemParameters",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                "MiseHorsBabasseAutoActivee",
                "SystemParameters");

            migrationBuilder.DropColumn(
                "MiseHorsBabasseHebdomadaireHeure",
                "SystemParameters");

            migrationBuilder.DropColumn(
                "MiseHorsBabasseHebdomadaireJours",
                "SystemParameters");

            migrationBuilder.DropColumn(
                "MiseHorsBabasseInstantanee",
                "SystemParameters");

            migrationBuilder.DropColumn(
                "MiseHorsBabasseQuotidienne",
                "SystemParameters");

            migrationBuilder.DropColumn(
                "MiseHorsBabasseQuotidienneHeure",
                "SystemParameters");

            migrationBuilder.AlterColumn<long>(
                "IdProduits",
                "TransactionHistory",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}