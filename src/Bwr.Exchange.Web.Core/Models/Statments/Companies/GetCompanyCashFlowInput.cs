namespace Bwr.Exchange.Models.Statments.Companies
{
    public class GetCompanyCashFlowPdfInput
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrentBalance { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
