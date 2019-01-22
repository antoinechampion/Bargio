//          Bargio - 20181030084116_ajout_TransactionsHistory.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class ajout_TransactionsHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                "TransactionHistory",
                table => new {
                    ID = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Montant = table.Column<decimal>(nullable: false),
                    IdProduit = table.Column<long>(nullable: true),
                    Commentaire = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_TransactionHistory", x => x.ID); });
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                "TransactionHistory");
        }
    }
}