//          Bargio - 20181211184432_update_TransactionsHistory2.cs
//  Copyright (c) Antoine Champion 2018-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class update_TransactionsHistory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AlterColumn<string>(
                "IdProduits",
                "TransactionHistory",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.AlterColumn<long>(
                "IdProduits",
                "TransactionHistory",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}