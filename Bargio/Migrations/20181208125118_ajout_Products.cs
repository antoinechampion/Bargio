//          Bargio - 20181208125118_ajout_Products.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class ajout_Products : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                "Product",
                table => new {
                    Id = table.Column<string>(nullable: false),
                    Nom = table.Column<string>(nullable: true),
                    Prix = table.Column<decimal>(nullable: false),
                    CompteurConsoMois = table.Column<long>(nullable: false),
                    CompteurConsoTotal = table.Column<long>(nullable: false),
                    RaccourciClavier = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Product", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                "Product");
        }
    }
}