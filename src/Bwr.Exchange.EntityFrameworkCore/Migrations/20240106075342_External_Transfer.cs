using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bwr.Exchange.Migrations
{
    public partial class External_Transfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "OutgoingTransfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ExtrenalTransfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromCompanyId = table.Column<int>(type: "int", nullable: true),
                    ToCompanyId = table.Column<int>(type: "int", nullable: true),
                    FromClientId = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    BeneficiaryId = table.Column<int>(type: "int", nullable: true),
                    SenderId = table.Column<int>(type: "int", nullable: true),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Commission = table.Column<double>(type: "float", nullable: false),
                    CompanyCommission = table.Column<double>(type: "float", nullable: false),
                    ClientCommission = table.Column<double>(type: "float", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtrenalTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtrenalTransfers_Clients_FromClientId",
                        column: x => x.FromClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExtrenalTransfers_Companies_FromCompanyId",
                        column: x => x.FromCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExtrenalTransfers_Companies_ToCompanyId",
                        column: x => x.ToCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExtrenalTransfers_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtrenalTransfers_Customers_BeneficiaryId",
                        column: x => x.BeneficiaryId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExtrenalTransfers_Customers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExtrenalTransfers_BeneficiaryId",
                table: "ExtrenalTransfers",
                column: "BeneficiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtrenalTransfers_CurrencyId",
                table: "ExtrenalTransfers",
                column: "CurrencyId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtrenalTransfers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "OutgoingTransfers");
        }
    }
}
