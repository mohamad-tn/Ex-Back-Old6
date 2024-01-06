using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bwr.Exchange.Migrations
{
    public partial class modify_external_transfer1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExtrenalTransfers_Currencies_CurrencyId",
                table: "ExtrenalTransfers");

            migrationBuilder.DropIndex(
                name: "IX_ExtrenalTransfers_CurrencyId",
                table: "ExtrenalTransfers");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "ExtrenalTransfers");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyName",
                table: "ExtrenalTransfers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyName",
                table: "ExtrenalTransfers");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "ExtrenalTransfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExtrenalTransfers_CurrencyId",
                table: "ExtrenalTransfers",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExtrenalTransfers_Currencies_CurrencyId",
                table: "ExtrenalTransfers",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
