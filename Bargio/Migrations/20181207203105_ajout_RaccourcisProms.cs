//          Bargio - 20181207203105_ajout_RaccourcisProms.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class ajout_RaccourcisProms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                "PromsKeyboardShortcut",
                table => new {
                    Raccourci = table.Column<string>(nullable: false),
                    TBK = table.Column<string>(nullable: true),
                    Proms = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_PromsKeyboardShortcut", x => x.Raccourci); });
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                "PromsKeyboardShortcut");
        }
    }
}