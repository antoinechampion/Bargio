//          Bargio - 20190120182845_update_SystemParameters_ajout_tx_lydia.cs
//  Copyright (c) Antoine Champion 2019-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class update_SystemParameters_ajout_tx_lydia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<decimal>(
                "CommissionLydiaFixe",
                "SystemParameters",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                "CommissionLydiaVariable",
                "SystemParameters",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                "MinimumRechargementLydia",
                "SystemParameters",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                "CommissionLydiaFixe",
                "SystemParameters");

            migrationBuilder.DropColumn(
                "CommissionLydiaVariable",
                "SystemParameters");

            migrationBuilder.DropColumn(
                "MinimumRechargementLydia",
                "SystemParameters");
        }
    }
}