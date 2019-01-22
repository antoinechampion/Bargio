//          Bargio - 20190121231428_update_PromsKeyboardShortcut_ajout_ID.cs
//  Copyright (c) Antoine Champion 2019-2019.
//  Distributed under the Boost Software License, Version 1.0.
//     (See accompanying file LICENSE_1_0.txt or copy at
//           http://www.boost.org/LICENSE_1_0.txt)

using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class update_PromsKeyboardShortcut_ajout_ID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropPrimaryKey(
                "PK_PromsKeyboardShortcut",
                "PromsKeyboardShortcut");

            migrationBuilder.AlterColumn<string>(
                "Raccourci",
                "PromsKeyboardShortcut",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                "ID",
                "PromsKeyboardShortcut",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                "PK_PromsKeyboardShortcut",
                "PromsKeyboardShortcut",
                "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropPrimaryKey(
                "PK_PromsKeyboardShortcut",
                "PromsKeyboardShortcut");

            migrationBuilder.DropColumn(
                "ID",
                "PromsKeyboardShortcut");

            migrationBuilder.AlterColumn<string>(
                "Raccourci",
                "PromsKeyboardShortcut",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                "PK_PromsKeyboardShortcut",
                "PromsKeyboardShortcut",
                "Raccourci");
        }
    }
}