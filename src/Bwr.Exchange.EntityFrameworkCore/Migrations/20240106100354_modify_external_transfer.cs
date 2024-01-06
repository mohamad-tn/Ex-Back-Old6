using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bwr.Exchange.Migrations
{
    public partial class modify_external_transfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExtrenalTransfers_Clients_FromClientId",
                table: "ExtrenalTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtrenalTransfers_Companies_FromCompanyId",
                table: "ExtrenalTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtrenalTransfers_Companies_ToCompanyId",
                table: "ExtrenalTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtrenalTransfers_Customers_BeneficiaryId",
                table: "ExtrenalTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtrenalTransfers_Customers_SenderId",
                table: "ExtrenalTransfers");

            migrationBuilder.DropIndex(
                name: "IX_ExtrenalTransfers_BeneficiaryId",
                table: "ExtrenalTransfers");

            migrationBuilder.DropIndex(
                name: "IX_ExtrenalTransfers_FromClientId",
                table: "ExtrenalTransfers");

            migrationBuilder.DropIndex(
                name: "IX_ExtrenalTransfers_FromCompanyId",
                table: "ExtrenalTransfers");

            migrationBuilder.DropIndex(
                name: "IX_ExtrenalTransfers_SenderId",
                table: "ExtrenalTransfers");

            migrationBuilder.DropIndex(
                name: "IX_ExtrenalTransfers_ToCompanyId",
                table: "ExtrenalTransfers");

            migrationBuilder.DropColumn(
                name: "BeneficiaryId",
                table: "ExtrenalTransfers");

            migrationBuilder.DropColumn(
                name: "FromClientId",
                table: "ExtrenalTransfers");

            migrationBuilder.DropColumn(
                name: "FromCompanyId",
                table: "ExtrenalTransfers");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "ExtrenalTransfers");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "ExtrenalTransfers");

            migrationBuilder.RenameColumn(
                name: "ToCompanyId",
                table: "ExtrenalTransfers",
                newName: "FromTenantId");

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryName",
                table: "ExtrenalTransfers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderName",
                table: "ExtrenalTransfers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeneficiaryName",
                table: "ExtrenalTransfers");

            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "ExtrenalTransfers");

            migrationBuilder.RenameColumn(
                name: "FromTenantId",
                table: "ExtrenalTransfers",
                newName: "ToCompanyId");

            migrationBuilder.AddColumn<int>(
                name: "BeneficiaryId",
                table: "ExtrenalTransfers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FromClientId",
                table: "ExtrenalTransfers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FromCompanyId",
                table: "ExtrenalTransfers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "ExtrenalTransfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SenderId",
                table: "ExtrenalTransfers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExtrenalTransfers_BeneficiaryId",
                table: "ExtrenalTransfers",
                column: "BeneficiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtrenalTransfers_FromClientId",
                table: "ExtrenalTransfers",
                column: "FromClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtrenalTransfers_FromCompanyId",
                table: "ExtrenalTransfers",
                column: "FromCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtrenalTransfers_SenderId",
                table: "ExtrenalTransfers",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtrenalTransfers_ToCompanyId",
                table: "ExtrenalTransfers",
                column: "ToCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExtrenalTransfers_Clients_FromClientId",
                table: "ExtrenalTransfers",
                column: "FromClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExtrenalTransfers_Companies_FromCompanyId",
                table: "ExtrenalTransfers",
                column: "FromCompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExtrenalTransfers_Companies_ToCompanyId",
                table: "ExtrenalTransfers",
                column: "ToCompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExtrenalTransfers_Customers_BeneficiaryId",
                table: "ExtrenalTransfers",
                column: "BeneficiaryId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExtrenalTransfers_Customers_SenderId",
                table: "ExtrenalTransfers",
                column: "SenderId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
