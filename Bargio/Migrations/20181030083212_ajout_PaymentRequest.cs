//          Bargio - 20181030083212_ajout_PaymentRequest.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class ajout_PaymentRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                "PaymentRequest",
                table => new {
                    ID = table.Column<string>(nullable: false),
                    Montant = table.Column<decimal>(nullable: false),
                    DateDemande = table.Column<DateTime>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_PaymentRequest", x => x.ID); });
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                "PaymentRequest");
        }
    }
}