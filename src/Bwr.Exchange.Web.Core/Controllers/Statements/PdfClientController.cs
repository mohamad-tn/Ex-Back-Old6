using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System;
using Bwr.Exchange.Models.Statments.Clients;
using Bwr.Exchange.Models.Statments;
using System.Linq;
using Bwr.Exchange.CashFlows.ClientCashFlows;
using Bwr.Exchange.Shared.DataManagerRequests;
using Bwr.Exchange.CashFlows.ClientCashFlows.Dto;
using Syncfusion.EJ2.HeatMap;
using Bwr.Exchange.Settings;
using Bwr.Exchange.Models.Statments.Totals;

namespace Bwr.Exchange.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PdfClientController : ExchangeControllerBase
    {
        private readonly IClientCashFlowAppService _clientCashFlowAppService;

        public PdfClientController(IClientCashFlowAppService clientCashFlowAppService)
        {
            _clientCashFlowAppService = clientCashFlowAppService;
        }

        [HttpPost]
        public PdfResultOutput GetTotalClientBalance(GetTotalClientBalanceInput input)
        {
            var dateStr = DateTime.Now.ToString("dd-MM-yyyy-HH.mm");
            var fileName = $"clent-balance_{dateStr}";
            var path = $"wwwroot/statments/{fileName}.pdf";

            var currencies = InitialCurrencies(input.TotalClientBalancePdfs);
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    if (currencies.Count > 3)
                    {
                        page.Size(PageSizes.A4.Landscape());
                    }

                    page.ContentFromRightToLeft();

                    //Header
                    page.Header().Padding(10).Column(column =>
                    {
                        column.Item().AlignCenter().PaddingHorizontal(10).Text("كشف ارصدة العملاء").SemiBold().FontSize(20).FontFamily(Fonts.Calibri);
                        column.Item().Row(row =>
                        {
                            IContainer ParamsCellStyle(IContainer c, string backgroundColor)
                            {
                                return c
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Lighten1)
                                    .Background(backgroundColor)
                                    .PaddingVertical(5)
                                    .PaddingHorizontal(5)
                                    .AlignCenter()
                                    .AlignMiddle();
                            }
                            var date = DateTime.Parse(input.ToDate).ToString("dd-MM-yyyy");
                            row.ConstantItem(100).Element(ParamsTitleStyle).Text("إلى تاريخ").FontFamily(Fonts.Calibri);
                            row.ConstantItem(100).Element(ParamsValueStyle).Text(date).FontFamily(Fonts.Calibri);

                            IContainer ParamsTitleStyle(IContainer c) => ParamsCellStyle(c, Colors.Grey.Lighten3);
                            IContainer ParamsValueStyle(IContainer c) => ParamsCellStyle(c, Colors.White).ShowOnce();

                        });
                    });
                    //Content
                    page.Content()
                    .Padding(10)
                    .MinimalBox()
                    .Border(1)
                    .BorderColor(Colors.Grey.Lighten1)
                        .Table(table =>
                        {
                            IContainer DefaultCellStyle(IContainer c, string backgroundColor)
                            {
                                return c
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Lighten1)
                                    .Background(backgroundColor)
                                    .PaddingVertical(5)
                                    .PaddingHorizontal(10)
                                    .AlignCenter()
                                    .AlignMiddle();
                            }

                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                for (int i = 0; i < currencies.Count; i++)
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                }
                            });

                            table.Header(header =>
                            {
                                header.Cell().RowSpan(2).Element(CellStyle).Text("العميل").FontFamily(Fonts.Calibri);
                                for (int i = 0; i < currencies.Count; i++)
                                {
                                    header.Cell().ColumnSpan(2).Element(CellStyle).Text(currencies[i]).FontFamily(Fonts.Calibri);
                                }

                                for (int i = 0; i < currencies.Count; i++)
                                {
                                    header.Cell().Element(CellStyle).Text("له").FontFamily(Fonts.Calibri);
                                    header.Cell().Element(CellStyle).Text("عليه").FontFamily(Fonts.Calibri);
                                }

                                IContainer CellStyle(IContainer c) => DefaultCellStyle(c, Colors.Grey.Lighten3);
                            });

                            foreach (var item in input.TotalClientBalancePdfs)
                            {
                                table.Cell().Element(CellStyle).Text(item.ClientName).FontFamily(Fonts.Calibri);

                                // Currency0
                                if (currencies.Any(x => x == item.Currency0))
                                {
                                    table.Cell().Element(CellStyle).Text(item.Balance0 < 0 ? Math.Abs(item.Balance0).ToString("N0") : "").FontFamily(Fonts.Calibri);
                                    table.Cell().Element(CellStyle).Text(item.Balance0 > 0 ? item.Balance0.ToString("N0") : "").FontFamily(Fonts.Calibri);
                                }
                                // Currency1
                                if (currencies.Any(x => x == item.Currency1))
                                {
                                    table.Cell().Element(CellStyle).Text(item.Balance1 < 0 ? Math.Abs(item.Balance1).ToString("N0") : "").FontFamily(Fonts.Calibri);
                                    table.Cell().Element(CellStyle).Text(item.Balance1 > 0 ? item.Balance1.ToString("N0") : "").FontFamily(Fonts.Calibri);
                                }
                                // Currency2
                                if (currencies.Any(x => x == item.Currency2))
                                {
                                    table.Cell().Element(CellStyle).Text(item.Balance2 < 0 ? Math.Abs(item.Balance2).ToString("N0") : "").FontFamily(Fonts.Calibri);
                                    table.Cell().Element(CellStyle).Text(item.Balance2 > 0 ? item.Balance2.ToString("N0") : "").FontFamily(Fonts.Calibri);
                                }
                                // Currency3
                                if (currencies.Any(x => x == item.Currency3))
                                {
                                    table.Cell().Element(CellStyle).Text(item.Balance3 < 0 ? Math.Abs(item.Balance3).ToString("N0") : "").FontFamily(Fonts.Calibri);
                                    table.Cell().Element(CellStyle).Text(item.Balance3 > 0 ? item.Balance3.ToString("N0") : "").FontFamily(Fonts.Calibri);
                                }
                                // Currency4
                                if (currencies.Any(x => x == item.Currency4))
                                {
                                    table.Cell().Element(CellStyle).Text(item.Balance4 < 0 ? Math.Abs(item.Balance4).ToString("N0") : "").FontFamily(Fonts.Calibri);
                                    table.Cell().Element(CellStyle).Text(item.Balance4 > 0 ? item.Balance4.ToString("N0") : "").FontFamily(Fonts.Calibri);
                                }

                                IContainer CellStyle(IContainer c) => DefaultCellStyle(c, Colors.White).ShowOnce();
                            }
                            table.Footer(footer =>
                            {
                                var forHim0 = input.TotalClientBalancePdfs.Where(x => x.Balance0 < 0).Sum(x => x.Balance0);
                                var forHim1 = input.TotalClientBalancePdfs.Where(x => x.Balance1 < 0).Sum(x => x.Balance1);
                                var forHim2 = input.TotalClientBalancePdfs.Where(x => x.Balance2 < 0).Sum(x => x.Balance2);
                                var forHim3 = input.TotalClientBalancePdfs.Where(x => x.Balance3 < 0).Sum(x => x.Balance3);
                                var forHim4 = input.TotalClientBalancePdfs.Where(x => x.Balance4 < 0).Sum(x => x.Balance4);

                                var onHim0 = input.TotalClientBalancePdfs.Where(x => x.Balance0 > 0).Sum(x => x.Balance0);
                                var onHim1 = input.TotalClientBalancePdfs.Where(x => x.Balance1 > 0).Sum(x => x.Balance1);
                                var onHim2 = input.TotalClientBalancePdfs.Where(x => x.Balance2 > 0).Sum(x => x.Balance2);
                                var onHim3 = input.TotalClientBalancePdfs.Where(x => x.Balance3 > 0).Sum(x => x.Balance3);
                                var onHim4 = input.TotalClientBalancePdfs.Where(x => x.Balance4 > 0).Sum(x => x.Balance4);

                                var total0 = (onHim0 + forHim0) < 0 ? $"{Reverse(Math.Abs(onHim0 + forHim0).ToString("N0"))} / له" : $"{Reverse((onHim0 + forHim0).ToString("N0"))} / عليه";
                                var total1 = (onHim1 + forHim1) < 0 ? $"{Reverse(Math.Abs(onHim1 + forHim1).ToString("N0"))} / له" : $"{Reverse((onHim1 + forHim1).ToString("N0"))} / عليه"; ;
                                var total2 = (onHim2 + forHim2) < 0 ? $"{Reverse(Math.Abs(onHim2 + forHim2).ToString("N0"))} / له" : $"{Reverse((onHim2 + forHim2).ToString("N0"))} / عليه"; ;
                                var total3 = (onHim3 + forHim3) < 0 ? $"{Reverse(Math.Abs(onHim3 + forHim3).ToString("N0"))} / له" : $"{Reverse((onHim3 + forHim3).ToString("N0"))} / عليه"; ;
                                var total4 = (onHim4 + forHim4) < 0 ? $"{Reverse(Math.Abs(onHim4 + forHim4).ToString("N0"))} / له" : $"{Reverse((onHim4 + forHim4).ToString("N0"))} / عليه"; ;

                                footer.Cell().RowSpan(2).Element(CellStyle).Text("المجموع").FontFamily(Fonts.Calibri);

                                if (currencies.Count >= 1)
                                {
                                    footer.Cell().Element(CellStyle).Text((Math.Abs(forHim0)).ToString("N0")).FontFamily(Fonts.Calibri);
                                    footer.Cell().Element(CellStyle).Text(onHim0.ToString("N0")).FontFamily(Fonts.Calibri);
                                }
                                if (currencies.Count >= 2)
                                {
                                    footer.Cell().Element(CellStyle).Text((Math.Abs(forHim1)).ToString("N0")).FontFamily(Fonts.Calibri);
                                    footer.Cell().Element(CellStyle).Text(onHim1.ToString("N0")).FontFamily(Fonts.Calibri);
                                }
                                if (currencies.Count >= 3)
                                {
                                    footer.Cell().Element(CellStyle).Text((Math.Abs(forHim2)).ToString("N0")).FontFamily(Fonts.Calibri);
                                    footer.Cell().Element(CellStyle).Text(onHim2.ToString("N0")).FontFamily(Fonts.Calibri);
                                }
                                if (currencies.Count >= 4)
                                {
                                    footer.Cell().Element(CellStyle).Text((Math.Abs(forHim3)).ToString("N0")).FontFamily(Fonts.Calibri);
                                    footer.Cell().Element(CellStyle).Text(onHim3.ToString("N0")).FontFamily(Fonts.Calibri);
                                }
                                if (currencies.Count >= 5)
                                {
                                    footer.Cell().Element(CellStyle).Text((Math.Abs(forHim4)).ToString("N0")).FontFamily(Fonts.Calibri);
                                    footer.Cell().Element(CellStyle).Text(onHim4.ToString("N0")).FontFamily(Fonts.Calibri);
                                }

                                if (currencies.Count >= 1)
                                    footer.Cell().ColumnSpan(2).Element(CellStyle).Text(total0).FontFamily(Fonts.Calibri);
                                if (currencies.Count >= 2)
                                    footer.Cell().ColumnSpan(2).Element(CellStyle).Text(total1).FontFamily(Fonts.Calibri);
                                if (currencies.Count >= 3)
                                    footer.Cell().ColumnSpan(2).Element(CellStyle).Text(total2).FontFamily(Fonts.Calibri);
                                if (currencies.Count >= 4)
                                    footer.Cell().ColumnSpan(2).Element(CellStyle).Text(total3).FontFamily(Fonts.Calibri);
                                if (currencies.Count >= 5)
                                    footer.Cell().ColumnSpan(2).Element(CellStyle).Text(total4).FontFamily(Fonts.Calibri);

                                IContainer CellStyle(IContainer c) => DefaultCellStyle(c, Colors.Grey.Lighten3);
                            });

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
        }

        [HttpPost]
        public PdfResultOutput GetClientCashFlow(GetClientCashFlowPdfInput input)
        {
            var dm = new CashFlowDataManagerRequest();
            dm.Skip = 0;
            dm.Take = 1000;
            dm.fromDate = input.FromDate;
            dm.toDate = input.ToDate;
            dm.currencyId = input.CurrencyId;
            dm.id = input.ClientId;

            var clientCashFlows = (IEnumerable<ClientCashFlowDto>)_clientCashFlowAppService.GetForGrid(dm).result;

            var dateStr = DateTime.Now.ToString("dd-MM-yyyy HH.mm");
            var fileName = $"client-cash-flow_{dateStr}";
            var path = $"wwwroot/statments/{fileName}.pdf";

            var lastCashFlow = clientCashFlows.LastOrDefault();
            var currentBalance = "0";
            if(lastCashFlow != null && lastCashFlow.Balance != 0)
            {
                var str = lastCashFlow.Balance < 0 ? $"{input.CurrencyName} / بذمتنا" : $"{input.CurrencyName} / بذمته";
                currentBalance = $"{Reverse(Math.Abs(lastCashFlow.Balance).ToString("N0"))} {str}";
            }

            var from = "";
            if (!string.IsNullOrEmpty(input.FromDate))
            {
                from = DateTime.Parse(input.FromDate).ToString("dd/MM/yyyy");
            }
            var to = "";
            if (!string.IsNullOrEmpty(input.ToDate))
            {
                to = DateTime.Parse(input.ToDate).ToString("dd/MM/yyyy");
            }

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
                                .AlignCenter()
                                .AlignMiddle();
                        }

                        column.Item().AlignCenter().PaddingBottom(20).Text("كشف رصيد العميل").SemiBold().FontSize(20).FontFamily(Fonts.Calibri);
                        column.Item().Row(row =>
                        {
                            //Client
                            row.ConstantItem(150).Element(ParamsTitleStyle).Text("العميل").FontFamily(Fonts.Calibri);
                            row.ConstantItem(150).Element(ParamsValueStyle).Text(input.ClientName).FontFamily(Fonts.Calibri);
                            //Space
                            //row.ConstantItem(50).Element(EmptySpaceStyle).Text("").FontFamily(Fonts.Calibri);
                            //Balance
                            row.ConstantItem(150).Element(ParamsTitleStyle).Text("الرصيد الحالي").FontFamily(Fonts.Calibri);
                            row.ConstantItem(150).Element(ParamsValueStyle).Text(currentBalance).FontFamily(Fonts.Calibri);

                        });
                        column.Item().Row(row =>
                        {
                            //From Date
                            row.ConstantItem(150).Element(ParamsTitleStyle).Text("من الفترة").FontFamily(Fonts.Calibri);
                            row.ConstantItem(150).Element(ParamsValueStyle).Text(from).FontFamily(Fonts.Calibri);
                            //Space
                            //row.ConstantItem(50).Element(EmptySpaceStyle).Text("").FontFamily(Fonts.Calibri);
                            //To Date
                            row.ConstantItem(150).Element(ParamsTitleStyle).Text("إلى").FontFamily(Fonts.Calibri);
                            row.ConstantItem(150).Element(ParamsValueStyle).Text(to).FontFamily(Fonts.Calibri);

                        });

                        IContainer ParamsTitleStyle(IContainer c) => ParamsCellStyle(c, Colors.Grey.Lighten3, 1);
                        IContainer ParamsValueStyle(IContainer c) => ParamsCellStyle(c, Colors.White, 1).ShowOnce();
                        IContainer EmptySpaceStyle(IContainer c) => ParamsCellStyle(c, Colors.White, 0).ShowOnce();
                    });
                    //Content
                    page.Content()
                    .Padding(10)
                    .MinimalBox()
                    .Border(1)
                    .BorderColor(Colors.Grey.Lighten1)
                        .Table(table =>
                        {
                            IContainer DefaultCellStyle(IContainer c, string backgroundColor)
                            {
                                return c
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Lighten1)
                                    .Background(backgroundColor)
                                    .PaddingVertical(5)
                                    .PaddingHorizontal(10)
                                    .AlignCenter()
                                    .AlignMiddle();
                            }

                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("الرصيد").FontFamily(Fonts.Calibri);
                                header.Cell().Element(CellStyle).Text("له").FontFamily(Fonts.Calibri);
                                header.Cell().Element(CellStyle).Text("عليه").FontFamily(Fonts.Calibri);
                                header.Cell().Element(CellStyle).Text("العمولة").FontFamily(Fonts.Calibri);
                                header.Cell().Element(CellStyle).Text("النوع").FontFamily(Fonts.Calibri);
                                header.Cell().Element(CellStyle).Text("التاريخ").FontFamily(Fonts.Calibri);
                                header.Cell().Element(CellStyle).Text("ملاحظات").FontFamily(Fonts.Calibri);

                                IContainer CellStyle(IContainer c) => DefaultCellStyle(c, Colors.Grey.Lighten3);
                            });
                            foreach (var item in clientCashFlows)
                            {
                                //الرصيد
                                string balance = "";
                                if (item.Balance != 0)
                                {
                                    balance = item.Balance < 0
                                    ? $"{Reverse(Math.Abs(item.Balance).ToString("N0"))} / له"
                                    : $"{Reverse((item.Balance).ToString("N0"))} / عليه";
                                }
                                table.Cell().Element(CellStyle).Text(balance).FontFamily(Fonts.Calibri);
                                //له
                                var forHim = item.Amount < 0 ? Math.Abs(item.Amount).ToString("N0") : string.Empty;
                                table.Cell().Element(CellStyle).Text(forHim).FontFamily(Fonts.Calibri);
                                //عليه
                                var onHim = item.Amount > 0 ? item.Amount.ToString("N0") : string.Empty;
                                table.Cell().Element(CellStyle).Text(onHim).FontFamily(Fonts.Calibri);
                                //العمولة
                                var commission = item.Commission > 0 ? item.Commission : item.ClientCommission;
                                var commisionStr = commission > 0 ? commission.ToString("N0") : string.Empty;
                                table.Cell().Element(CellStyle).Text(commisionStr).FontFamily(Fonts.Calibri);
                                //النوع
                                table.Cell().Element(CellStyle).Text(item.Type).FontFamily(Fonts.Calibri);
                                //التاريخ
                                var date = item.Date != null ? item.Date.Value.ToString("dd/MM/yyyy hh:mm") : string.Empty;
                                table.Cell().Element(CellStyle).Text(date).FontFamily(Fonts.Calibri);
                                //ملاحظات
                                table.Cell().Element(CellStyle).Text(item.Note).FontFamily(Fonts.Calibri);

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
        }

        private List<string> InitialCurrencies(IList<TotalClientBalancePdf> totalClientBalancePdfs)
        {
            var currencies = new List<string>();
            TotalClientBalancePdf clientBalancePdf = null;
            clientBalancePdf = totalClientBalancePdfs.FirstOrDefault(x => !string.IsNullOrEmpty(x.Currency0));
            if (clientBalancePdf != null)
                currencies.Add(clientBalancePdf.Currency0);

            clientBalancePdf = totalClientBalancePdfs.FirstOrDefault(x => !string.IsNullOrEmpty(x.Currency1));
            if (clientBalancePdf != null)
                currencies.Add(clientBalancePdf.Currency1);

            clientBalancePdf = totalClientBalancePdfs.FirstOrDefault(x => !string.IsNullOrEmpty(x.Currency2));
            if (clientBalancePdf != null)
                currencies.Add(clientBalancePdf.Currency2);

            clientBalancePdf = totalClientBalancePdfs.FirstOrDefault(x => !string.IsNullOrEmpty(x.Currency3));
            if (clientBalancePdf != null)
                currencies.Add(clientBalancePdf.Currency3);

            clientBalancePdf = totalClientBalancePdfs.FirstOrDefault(x => !string.IsNullOrEmpty(x.Currency4));
            if (clientBalancePdf != null)
                currencies.Add(clientBalancePdf.Currency4);

            return currencies;
        }

        string Reverse(string txt)
        {
            char[] chararray = txt.ToCharArray();
            Array.Reverse(chararray);
            string reverseTxt = "";
            for (int i = 0; i <= chararray.Length - 1; i++)
            {
                reverseTxt += chararray.GetValue(i);
            }
            return reverseTxt;
        }
    }
}

