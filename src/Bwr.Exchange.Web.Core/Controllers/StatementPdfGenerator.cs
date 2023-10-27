using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Bwr.Exchange.Controllers
{
    [Route("api/[controller]/[action]")]

    public class StatementPdfGenerator : ExchangeControllerBase
    {
        [HttpPost]
        public void TotalClientBalanceStatement()
        {

            Document.Create(container =>
            {

                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Background(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));
                    page.ContentFromRightToLeft();

                    page.Header()
                        .Text("كشف ارصدة الشركات").DirectionFromRightToLeft()
                        .SemiBold().FontSize(18).FontColor(Colors.Black)
                        .FontFamily(Fonts.Arial);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre).Padding(10)

                    .Table(x =>
                    {

                        IContainer DefaultCellStyle(IContainer container1, string backgroundColor)
                        {
                            return container1
                                .Border(1)
                                .BorderColor(Colors.Grey.Lighten1)
                                .Background(backgroundColor)
                                .PaddingVertical(2)
                                .PaddingHorizontal(4)
                                .AlignCenter()
                                .AlignMiddle();
                        }


                        x.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        x.Header(header =>
                        {
                            // please be sure to call the 'header' handler!

                            header.Cell().RowSpan(2).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("الشركة").FontFamily(Fonts.Calibri);

                            header.Cell().ColumnSpan(2).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("دولار").FontFamily(Fonts.Calibri);
                            header.Cell().ColumnSpan(2).Element(CellStyle).ExtendHorizontal().AlignCenter().Text("دينار").FontFamily(Fonts.Calibri);

                            header.Cell().Element(CellStyle).ExtendHorizontal().AlignCenter().Text("له").FontFamily(Fonts.Calibri);
                            header.Cell().Element(CellStyle).ExtendHorizontal().AlignCenter().Text("عليه").FontFamily(Fonts.Calibri);

                            header.Cell().Element(CellStyle).ExtendHorizontal().AlignCenter().Text("له").FontFamily(Fonts.Calibri);
                            header.Cell().Element(CellStyle).ExtendHorizontal().AlignCenter().Text("عليه").FontFamily(Fonts.Calibri);
                            IContainer CellStyle(IContainer container2) => DefaultCellStyle(container2, Colors.Grey.Lighten3);
                        });

                        //for (int i = 0; i < 8; i++)
                        //{
                            x.Cell().Element(CellStyle1).ExtendHorizontal().AlignCenter().Text("الهرم-سوريا").FontSize(12).FontFamily(Fonts.Calibri);
                            x.Cell().Element(CellStyle1).ExtendHorizontal().AlignCenter().Text("3000000").FontSize(12).FontFamily(Fonts.Calibri);
                            x.Cell().Element(CellStyle1).ExtendHorizontal().AlignCenter().Text("").FontSize(12).FontFamily(Fonts.Calibri);
                            x.Cell().Element(CellStyle1).ExtendHorizontal().AlignCenter().Text("12000000").FontSize(12).FontFamily(Fonts.Calibri);
                            x.Cell().Element(CellStyle1).ExtendHorizontal().AlignCenter().Text("").FontSize(12).FontFamily(Fonts.Calibri);
                            x.Cell().Element(CellStyle1).ExtendHorizontal().AlignCenter().Text("").FontSize(12).FontFamily(Fonts.Calibri);
                            x.Cell().Element(CellStyle1).ExtendHorizontal().AlignCenter().Text("").FontSize(12).FontFamily(Fonts.Calibri);



                        //x.Cell().RowSpan(2).ExtendHorizontal().AlignCenter().Text("المحموع").FontFamily(Fonts.Calibri);
                        //x.Cell().ColumnSpan(2).ExtendHorizontal().AlignCenter().Text("122222").FontFamily(Fonts.Calibri);
                        //x.Cell().ColumnSpan(2).ExtendHorizontal().AlignCenter().Text("565665").FontFamily(Fonts.Calibri);
                        IContainer CellStyle1(IContainer container2) => DefaultCellStyle(container2, Colors.Grey.Lighten3);
                        //}

                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            })
                    .GeneratePdf("hello.pdf");

            Process.Start("explorer.exe", "hello.pdf");

        }

    }
}
