using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System;
using Bwr.Exchange.Models.Statments;
using System.Linq;
using Bwr.Exchange.Models.Statments.Companies;
using Bwr.Exchange.Models.Statments.Totals;

namespace Bwr.Exchange.Controllers.Statements
{
    [Route("api/[controller]/[action]")]
    public class PdfClientAndCompanyController: PdfStatmentControllerBase
    {
        [HttpPost]
        public PdfResultOutput GetTotalBalance(GetTotalBalanceInput input)
        {
            //var dateStr = DateTime.Now.ToString("dd-MM-yyyy-HH.mm");
            //var fileName = $"companies-and-clients-balance_{dateStr}";
            var fileName = $"companies-and-clients-balance";
            var path = $"wwwroot/statments/{fileName}.pdf";

            var currencies = InitialCurrencies(input.TotalBalancePdfs);
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
                        column.Item().AlignCenter().PaddingHorizontal(10).Text("كشف ارصدة العملاء والشركات").SemiBold().FontSize(20).FontFamily(Fonts.Calibri);
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

                            foreach (var item in input.TotalBalancePdfs)
                            {
                                table.Cell().Element(CellStyle).Text(item.Name).FontFamily(Fonts.Calibri);

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
                                var forHim0 = input.TotalBalancePdfs.Where(x => x.Balance0 < 0).Sum(x => x.Balance0);
                                var forHim1 = input.TotalBalancePdfs.Where(x => x.Balance1 < 0).Sum(x => x.Balance1);
                                var forHim2 = input.TotalBalancePdfs.Where(x => x.Balance2 < 0).Sum(x => x.Balance2);
                                var forHim3 = input.TotalBalancePdfs.Where(x => x.Balance3 < 0).Sum(x => x.Balance3);
                                var forHim4 = input.TotalBalancePdfs.Where(x => x.Balance4 < 0).Sum(x => x.Balance4);

                                var onHim0 = input.TotalBalancePdfs.Where(x => x.Balance0 > 0).Sum(x => x.Balance0);
                                var onHim1 = input.TotalBalancePdfs.Where(x => x.Balance1 > 0).Sum(x => x.Balance1);
                                var onHim2 = input.TotalBalancePdfs.Where(x => x.Balance2 > 0).Sum(x => x.Balance2);
                                var onHim3 = input.TotalBalancePdfs.Where(x => x.Balance3 > 0).Sum(x => x.Balance3);
                                var onHim4 = input.TotalBalancePdfs.Where(x => x.Balance4 > 0).Sum(x => x.Balance4);

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

        private List<string> InitialCurrencies(IList<TotalBalancePdf> totalCompanyBalances)
        {
            var currencies = new List<string>();
            TotalBalancePdf balancePdf = null;
            balancePdf = totalCompanyBalances.FirstOrDefault(x => !string.IsNullOrEmpty(x.Currency0));
            if (balancePdf != null)
                currencies.Add(balancePdf.Currency0);

            balancePdf = totalCompanyBalances.FirstOrDefault(x => !string.IsNullOrEmpty(x.Currency1));
            if (balancePdf != null)
                currencies.Add(balancePdf.Currency1);

            balancePdf = totalCompanyBalances.FirstOrDefault(x => !string.IsNullOrEmpty(x.Currency2));
            if (balancePdf != null)
                currencies.Add(balancePdf.Currency2);

            balancePdf = totalCompanyBalances.FirstOrDefault(x => !string.IsNullOrEmpty(x.Currency3));
            if (balancePdf != null)
                currencies.Add(balancePdf.Currency3);

            balancePdf = totalCompanyBalances.FirstOrDefault(x => !string.IsNullOrEmpty(x.Currency4));
            if (balancePdf != null)
                currencies.Add(balancePdf.Currency4);

            return currencies;
        }

        
    }
}
