using Microsoft.EntityFrameworkCore.Migrations;

namespace Bwr.Exchange.Migrations
{
    public partial class Update_Management_Statement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ToCompanyId",
                table: "ManagementStatement",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ManagementStatement_ToCompanyId",
                table: "ManagementStatement",
                column: "ToCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManagementStatement_Companies_ToCompanyId",
                table: "ManagementStatement",
                column: "ToCompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManagementStatement_Companies_ToCompanyId",
                table: "ManagementStatement");

            migrationBuilder.DropIndex(
                name: "IX_ManagementStatement_ToCompanyId",
                table: "ManagementStatement");

            migrationBuilder.DropColumn(
                name: "ToCompanyId",
                table: "ManagementStatement");
        }
    }
}
