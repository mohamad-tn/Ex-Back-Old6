namespace Bwr.Exchange.Settings.Companies.Dto
{
    public class ReadCompanyDto
    {
        public int id { get; set; }
        public int? tenantCompanyId { get; set; }
        public int? tenantId { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
    }
}
