//          Bargio - 20181103104549_update_UserData.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class update_UserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<string>(
                "Nom",
                "UserData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "Prenom",
                "UserData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "Telephone",
                "UserData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                "Nom",
                "UserData");

            migrationBuilder.DropColumn(
                "Prenom",
                "UserData");

            migrationBuilder.DropColumn(
                "Telephone",
                "UserData");
        }
    }
}