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
using Bwr.Exchange.TreasuryActions.Dto;

namespace Bwr.Exchange.Controllers.Statements
{
    [Route("api/[controller]/[action]")]
    public class PdfTreasuryBalanceController : PdfStatmentControllerBase
    {
        private readonly TreasuryCashFlowAppService _teasuryCashFlowAppService;

        public PdfTreasuryBalanceController(TreasuryCashFlowAppService teasuryCashFlowAppService)
        {
            _teasuryCashFlowAppService = teasuryCashFlowAppService;
        }

        [HttpPost]
        public PdfResultOutput GetTreasuryCashFlow(GetTreasuryCashFlowPdfInput input)
        {
            var dm = new DataManagerRequest();
            dm.Skip = 0;
            dm.Take = 1000;
            var filters = new List<WhereFilter>()
            {
                new WhereFilter()
                {
                    IgnoreCase = false,
                    predicates = new List<WhereFilter>()
                }
            };
            filters.First().predicates.Add(new WhereFilter()
            {
                Condition = "equal",
                value = input.FromDate,
                Field = "fromDate",
                IgnoreCase = true
            });
            filters.First().predicates.Add(new WhereFilter()
            {
                Condition = "equal",
                value = input.CurrencyId,
                Field = "currencyId",
                IgnoreCase = true
            });
            filters.First().predicates.Add(new WhereFilter()
            {
                Condition = "equal",
                value = input.ToDate,
                Field = "toDate",
                IgnoreCase = true
            });
            dm.Where = filters;
            var tearuryCashFlows = (IEnumerable<TreasuryCashFlowDto>)_teasuryCashFlowAppService.GetForGrid(dm).result;

            var dateStr = DateTime.Now.ToString("dd-MM-yyyy-HH-mm");
            var fileName = $"treasury-balance_{dateStr}";
            var path = $"wwwroot/statments/{fileName}.pdf";

            var lastCashFlow = tearuryCashFlows.LastOrDefault();
            var currentBalance = "0";
            if (lastCashFlow != null && lastCashFlow.Balance != 0)
            {
                currentBalance = $"{Reverse(Math.Abs(lastCashFlow.Balance).ToString("N0"))}";
            }

            var from = "";
            if (!string.IsNullOrEmpty(input.FromDate))
            {
                from = DateTime.Parse(input.FromDate).ToString("dd/MM/yyyy");
                from = Reverse(from);
            }
            var to = "";
            if (!string.IsNullOrEmpty(input.ToDate))
            {
                to = DateTime.Parse(input.ToDate).ToString("dd/MM/yyyy");
                to = Reverse(to);
            }

            var currencyName = lastCashFlow != null ? lastCashFlow.Currency?.Name : "";

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());

                    page.ContentFromRightToLeft();

                    //Header
                    page.Header().AlignCenter().Padding(10).Column(column =>
                    {
                        IContainer ParamsCellStyle(IContainer c, string backgroundColor, int border)
                        {
                            return c
                                .Border(border)
                                .BorderColor(Colors.Grey.Lighten1)
                                .Background(backgroundColor)
                                .PaddingVertical(2)
                                .PaddingHorizontal(5)
                                .AlignRight();
                        }
                        //IContainer TitleCellStyle(IContainer c, string backgroundColor, int border)
                        //{
                        //    return c
                        //        .Border(border)
                        //        .BorderColor(Colors.Grey.Lighten1)
                        //        .Background(backgroundColor)
                        //        .PaddingVertical(2)
                        //        .PaddingHorizontal(5)
                        //        .AlignCenter()
                        //        .AlignMiddle();
                        //}

                        column.Item().Row(row =>
                        {
                            var currencyTitle = "العملة" + ": " + currencyName;
                            row.RelativeItem().Element(ParamStyle).Text(currencyTitle).FontFamily(Fonts.Calibri);
                            //row.ConstantItem(50).Element(EmptySpaceStyle).Text("").FontFamily(Fonts.Calibri);
                        });
                        column.Item().Row(row =>
                        {
                            var fromTitle = $"من تاريخ: {from}";
                            row.RelativeItem().Element(ParamStyle).Text(fromTitle).FontFamily(Fonts.Calibri);
                        });
                        column.Item().Row(row =>
                        {
                            var toTitle = $"إلى تاريخ: {to}";
                            row.RelativeItem().Element(ParamStyle).Text(toTitle).FontFamily(Fonts.Calibri);
                        });

                        column.Item().AlignCenter().PaddingBottom(6).Text("كشف رصيد الصندوق").SemiBold().FontSize(20).FontFamily(Fonts.Calibri);

                        //IContainer TitleStyle(IContainer c) => TitleCellStyle(c, Colors.Grey.Lighten3, 1);
                        IContainer ParamStyle(IContainer c) => ParamsCellStyle(c, Colors.White, 0).ShowOnce();
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
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                //columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("الرصيد").FontFamily(Fonts.Calibri);
                                header.Cell().Element(CellStyle).Text("الوارد").FontFamily(Fonts.Calibri);
                                header.Cell().Element(CellStyle).Text("الصادر").FontFamily(Fonts.Calibri);
                                header.Cell().Element(CellStyle).Text("النوع").FontFamily(Fonts.Calibri);
                                header.Cell().Element(CellStyle).Text("الاسم").FontFamily(Fonts.Calibri);
                                header.Cell().Element(CellStyle).Text("التاريخ").FontFamily(Fonts.Calibri);
                                //header.Cell().Element(CellStyle).Text("ملاحظات").FontFamily(Fonts.Calibri);

                                IContainer CellStyle(IContainer c) => DefaultCellStyle(c, Colors.Grey.Lighten3);
                            });
                            foreach (var item in tearuryCashFlows)
                            {
                                //الرصيد
                                string balance = "";
                                if (item.Balance != 0)
                                {
                                    balance = $"{Math.Abs(item.Balance).ToString("N0")}";
                                }
                                table.Cell().Element(CellStyle).Text(balance).FontFamily(Fonts.Calibri);
                                //الوارد
                                var forHim = item.Amount > 0 ? Math.Abs(item.Amount).ToString("N0") : string.Empty;
                                table.Cell().Element(CellStyle).Text(forHim).FontFamily(Fonts.Calibri);
                                //الصادر
                                var onHim = item.Amount < 0 ? item.Amount.ToString("N0") : string.Empty;
                                table.Cell().Element(CellStyle).Text(onHim).FontFamily(Fonts.Calibri);
                                //العمولة
                                //var commission = item.Commission > 0 ? item.Commission : item.tearuryCommission;
                                //var commisionStr = commission > 0 ? commission.ToString("N0") : string.Empty;
                                //table.Cell().Element(CellStyle).Text(commisionStr).FontFamily(Fonts.Calibri);
                                //النوع
                                table.Cell().Element(CellStyle).Text(item.Type).FontFamily(Fonts.Calibri);
                                //الاسم
                                table.Cell().Element(CellStyle).Text(item.Type).FontFamily(Fonts.Calibri);
                                //التاريخ
                                var date = item.Date != null ? item.Date.Value.ToString("dd/MM/yyyy hh:mm") : string.Empty;
                                table.Cell().Element(CellStyle).Text(date).FontFamily(Fonts.Calibri);
                                //ملاحظات
                                //table.Cell().Element(CellStyle).Text(item.Note).FontFamily(Fonts.Calibri);

                                IContainer CellStyle(IContainer c) => DefaultCellStyle(c, Colors.White).ShowOnce();
                            }
                            //table.Footer(footer =>
                            //{

                            //    IContainer CellStyle(IContainer c) => DefaultCellStyle(c, Colors.Grey.Lighten3);
                            //});

                        });
                    //Footer
                    page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("الصفحة ").FontFamily(Fonts.Calibri);
                        x.CurrentPageNumber();
                    });

                });
            })
            .GeneratePdf(path);

            return new PdfResultOutput($"statments/{fileName}.pdf");
            //return new PdfResultOutput($"statments/fileName.pdf");
        }
    }
}
