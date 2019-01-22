//          Bargio - 20181207220342_ajou_SystemParameters.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class ajou_SystemParameters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                "SystemParameters",
                table => new {
                    IpServeur = table.Column<string>(nullable: false),
                    DerniereConnexionBabasse = table.Column<DateTime>(nullable: false),
                    BucquagesBloques = table.Column<bool>(nullable: false),
                    LydiaBloque = table.Column<bool>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_SystemParameters", x => x.IpServeur); });
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                "SystemParameters");
        }
    }
}