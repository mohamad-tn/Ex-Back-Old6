using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bwr.Exchange.Migrations
{
    public partial class add_isCopied : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCopied",
                table: "OutgoingTransfers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCopied",
                table: "OutgoingTransfers");
        }
    }
}
