using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Bwr.Exchange.Models.Statments;
using System.Linq;
using Bwr.Exchange.CashFlows.TreasuryCashFlows;
using Bwr.Exchange.CashFlows.TreasuryCashFlows.Dto;
using Bwr.Exchange.Models.Statments.Treasuries;
using Syncfusion.EJ2.Base;
using System;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using Bwr.Exchange.TreasuryActions.Services;
using Bwr.Exchange.TreasuryActions;
using Bwr.Exchange.TreasuryActions.Dto;
using System.IO;

namespace Bwr.Exchange.Controllers.Statements
{
    [Route("api/[controller]/[action]")]
    public class PdfTreasuryActionController : PdfStatmentControllerBase
    {
        private readonly ITreasuryActionAppService _treasuryActionAppService;

        public PdfTreasuryActionController(ITreasuryActionAppService treasuryActionAppService)
        {
            _treasuryActionAppService = treasuryActionAppService;
        }

        [HttpPost]
        public PdfResultOutput GetTreasuryAction(TreasuryActionStatementInputDto input)
        {
            var dateStr = DateTime.Now.ToString("dd-MM-yyyy-HH-mm");
            var pdfName = input.ActionType == 1 ? "Spends" : "Incoms";
            var fileName = $"{pdfName}_{dateStr}";
            var path = $"wwwroot/statments/{fileName}.pdf";
            var title = input.ActionType == 1 ? "كشف تحليل المقبوضات" : this.L("ReceiptsStatment");
            var treasuryActions = _treasuryActionAppService.GetFroStatment(input);
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());

                    page.ContentFromRightToLeft();

                    //Header
                    page.Header().AlignCenter().Padding(10).Column(column =>
                    {
                        column.Item().AlignCenter().PaddingBottom(6).Text(title).SemiBold().FontSize(20).FontFamily(Fonts.Arial);
                    });
                    //Content
                    page.Content()
                    .Padding(10)
                    .MinimalBox()
                    .Border(1)
                    .BorderColor(Colors.Grey.Lighten1)
                        .Table(table =>
                        {

                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn(2);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("المبلغ").FontFamily(Fonts.Arial);
                                header.Cell().Element(CellStyle).Text("العملة").FontFamily(Fonts.Arial);
                                header.Cell().Element(CellStyle).Text("الحساب الرئيسي").FontFamily(Fonts.Arial);
                                header.Cell().Element(CellStyle).Text("الاسم").FontFamily(Fonts.Arial);
                                header.Cell().Element(CellStyle).Text("رقم السند").FontFamily(Fonts.Arial);
                                header.Cell().Element(CellStyle).Text("التاريخ").FontFamily(Fonts.Arial);
                                header.Cell().Element(CellStyle).Text("ملاحظات").FontFamily(Fonts.Arial);

                                IContainer CellStyle(IContainer c) => DefaultCellStyle(c, Colors.Grey.Lighten3);
                            });
                            foreach (var item in treasuryActions)
                            {
                                //المبلغ
                                string amount = "";
                                if (item.Amount != 0)
                                {
                                    amount = $"{Math.Abs(item.Amount).ToString("N0")}";
                                }
                                table.Cell().Element(CellStyle).Text(amount).FontFamily(Fonts.Arial);
                                //العملة
                                var currency = item.Currency?.Name;
                                table.Cell().Element(CellStyle).Text(currency).FontFamily(Fonts.Arial);
                                //الحساب الرئيسي
                                var mainAcount = "";
                                switch (item.MainAccount)
                                {
                                    case 0: mainAcount = "ذمم عملاء"; break;
                                    case 1: mainAcount = "ذمم شركات"; break;
                                    case 2: mainAcount = "إيرادات عامة"; break;
                                    case 3: mainAcount = "مصاريف عامة"; break;
                                    case 4: mainAcount = "حوالات مباشرة"; break;
                                }
                                table.Cell().Element(CellStyle).Text(mainAcount).FontFamily(Fonts.Arial);
                                //الاسم
                                var name = "";
                                switch (item.MainAccount)
                                {
                                    case 0: mainAcount = item.MainAccountClient?.Name; break;
                                    case 1: mainAcount = item.MainAccountCompany?.Name; break;
                                    case 2: mainAcount = item.Income?.Name; break;
                                    case 3: mainAcount = item.Expense?.Name; break;
                                    case 4: mainAcount = item.IncomeTransferDetail.Beneficiary?.Name; break;
                                }
                                table.Cell().Element(CellStyle).Text(name).FontFamily(Fonts.Arial);
                                //رقم السند
                                var number = item.Number.ToString();
                                table.Cell().Element(CellStyle).Text(number).FontFamily(Fonts.Arial);
                                //التاريخ
                                var date = item.Date != null ? item.Date : string.Empty;
                                table.Cell().Element(CellStyle).Text(date).FontFamily(Fonts.Arial);
                                //ملاحظات
                                table.Cell().Element(CellStyle).Text(item.Note).FontFamily(Fonts.Arial);

                                IContainer CellStyle(IContainer c) => DefaultCellStyle(c, Colors.White).ShowOnce();
                            }
                            table.Footer(footer =>
                            {
                                var total = treasuryActions.Sum(x => x.Amount).ToString("N0");
                                var totalText = 
                                footer.Cell().ColumnSpan(4).Element(CellStyle).Text(total).FontFamily(Fonts.Arial);
                                footer.Cell().ColumnSpan(3).Element(CellStyle).Text("الاجمالي").FontFamily(Fonts.Arial);
                                IContainer CellStyle(IContainer c) => DefaultCellStyle(c, Colors.Grey.Lighten3);
                            });

                        });
                    //Footer
                    page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("الصفحة ").FontFamily(Fonts.Arial);
                        x.CurrentPageNumber();
                    });

                });
            })
            .GeneratePdf(path);

            return new PdfResultOutput($"statments/{fileName}.pdf");
        }
    }
}
