using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;

namespace Bwr.Exchange.Controllers.Statements
{
    public class PdfStatmentControllerBase : ExchangeControllerBase
    {
        protected string Reverse(string txt)
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

        protected static IContainer DefaultCellStyle(IContainer c, string backgroundColor)
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
    }
}
