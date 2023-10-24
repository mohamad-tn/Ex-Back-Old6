using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bwr.Exchange.Migrations
{
    public partial class Add_Management_Statement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManagementStatement",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    PaymentType = table.Column<int>(nullable: true),
                    ChangeDate = table.Column<DateTime>(nullable: false),
                    ChangeType = table.Column<int>(nullable: false),
                    Number = table.Column<double>(nullable: true),
                    TreasuryActionType = table.Column<int>(nullable: true),
                    MainAccount = table.Column<string>(nullable: true),
                    BeforChange = table.Column<string>(nullable: true),
                    AfterChange = table.Column<string>(nullable: true),
                    AmountOfFirstCurrency = table.Column<double>(nullable: true),
                    AmoutOfSecondCurrency = table.Column<double>(nullable: true),
                    PaidAmountOfFirstCurrency = table.Column<double>(nullable: true),
                    ReceivedAmountOfFirstCurrency = table.Column<double>(nullable: true),
                    PaidAmountOfSecondCurrency = table.Column<double>(nullable: true),
                    ReceivedAmountOfSecondCurrency = table.Column<double>(nullable: true),
                    Commission = table.Column<double>(nullable: true),
                    FirstCurrencyId = table.Column<int>(nullable: true),
                    SecondCurrencyId = table.Column<int>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: true),
                    ClientId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    CompanyId = table.Column<int>(nullable: true),
                    SenderId = table.Column<int>(nullable: true),
                    BeneficiaryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagementStatement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManagementStatement_Customers_BeneficiaryId",
                        column: x => x.BeneficiaryId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ManagementStatement_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ManagementStatement_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ManagementStatement_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ManagementStatement_Currencies_FirstCurrencyId",
                        column: x => x.FirstCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ManagementStatement_Currencies_SecondCurrencyId",
                        column: x => x.SecondCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ManagementStatement_Customers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ManagementStatement_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManagementStatement_BeneficiaryId",
                table: "ManagementStatement",
                column: "BeneficiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementStatement_ClientId",
                table: "ManagementStatement",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementStatement_CompanyId",
                table: "ManagementStatement",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementStatement_CurrencyId",
                table: "ManagementStatement",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementStatement_FirstCurrencyId",
                table: "ManagementStatement",
                column: "FirstCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementStatement_SecondCurrencyId",
                table: "ManagementStatement",
                column: "SecondCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementStatement_SenderId",
                table: "ManagementStatement",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementStatement_UserId",
                table: "ManagementStatement",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManagementStatement");
        }
    }
}
