namespace Bwr.Exchange.Models.Statments.Totals
{
    public class GetCashFlowPdfInput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrentBalance { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
