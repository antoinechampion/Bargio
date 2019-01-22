//          Bargio - 20181201122542_ajout_hash_UserData.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class ajout_hash_UserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<bool>(
                "FoysApiHasPassword",
                "UserData",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                "FoysApiPasswordHash",
                "UserData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                "FoysApiHasPassword",
                "UserData");

            migrationBuilder.DropColumn(
                "FoysApiPasswordHash",
                "UserData");
        }
    }
}