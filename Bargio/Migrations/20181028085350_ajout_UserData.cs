//          Bargio - 20181028085350_ajout_UserData.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class ajout_UserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                "UserData",
                table => new {
                    UserName = table.Column<string>(nullable: false),
                    Solde = table.Column<decimal>(nullable: false),
                    Nums = table.Column<long>(nullable: false),
                    TBK = table.Column<string>(nullable: true),
                    Proms = table.Column<long>(nullable: false),
                    HorsFoys = table.Column<bool>(nullable: false),
                    Surnom = table.Column<string>(nullable: true),
                    DateDerniereModif = table.Column<DateTime>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_UserData", x => x.UserName); });
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                "UserData");
        }
    }
}