using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bwr.Exchange.Migrations
{
    public partial class modify_external_transfer2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromTenantName",
                table: "ExtrenalTransfers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OutgoingTransferId",
                table: "ExtrenalTransfers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromTenantName",
                table: "ExtrenalTransfers");

            migrationBuilder.DropColumn(
                name: "OutgoingTransferId",
                table: "ExtrenalTransfers");
        }
    }
}
