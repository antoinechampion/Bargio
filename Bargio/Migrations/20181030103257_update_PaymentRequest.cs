using Microsoft.EntityFrameworkCore.Migrations;

namespace Bargio.Migrations
{
    public partial class update_PaymentRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "PaymentRequest",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "PaymentRequest");
        }
    }
}
