using System.Collections.Generic;

namespace Bwr.Exchange.Models.Statments.Companies
{
    public class GetTotalCompanyBalanceInput
    {
        public GetTotalCompanyBalanceInput()
        {
            TotalCompanyBalancePdfs = new List<TotalCompanyBalancePdf>();
        }

        public string ToDate { get; set; }
        public IList<TotalCompanyBalancePdf> TotalCompanyBalancePdfs { get; set; }
    }
}
