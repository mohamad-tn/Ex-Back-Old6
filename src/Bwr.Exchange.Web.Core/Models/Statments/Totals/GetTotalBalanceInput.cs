using System.Collections.Generic;

namespace Bwr.Exchange.Models.Statments.Totals
{
    public class GetTotalBalanceInput
    {
        public GetTotalBalanceInput()
        {
            TotalBalancePdfs = new List<TotalBalancePdf>();
        }

        public string ToDate { get; set; }
        public IList<TotalBalancePdf> TotalBalancePdfs { get; set; }
    }
}
