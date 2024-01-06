namespace Bwr.Exchange.Models.Statments.Treasuries
{
    public class GetTreasuryCashFlowPdfInput
    {
        public int CurrencyId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
