namespace Bwr.Exchange.Models.Statments.Clients
{
    public class GetClientCashFlowPdfInput
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrentBalance { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
