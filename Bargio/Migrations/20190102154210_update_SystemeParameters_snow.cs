//          Bargio - 20190102154210_update_SystemeParameters_snow.cs
//  Copyright (c) Antoine Champion 2019-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class update_SystemeParameters_snow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<string>(
                "Actualites",
                "SystemParameters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "MotDesZifoys",
                "SystemParameters",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                "Snow",
                "SystemParameters",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                "Actualites",
                "SystemParameters");

            migrationBuilder.DropColumn(
                "MotDesZifoys",
                "SystemParameters");

            migrationBuilder.DropColumn(
                "Snow",
                "SystemParameters");
        }
    }
}