using Bwr.Exchange.Models.Statments.Clients;
using System.Collections.Generic;

namespace Bwr.Exchange.Models.Statments.Totals
{
    public class GetTotalClientBalanceInput
    {
        public GetTotalClientBalanceInput()
        {
            TotalClientBalancePdfs = new List<TotalClientBalancePdf>();
        }

        public string ToDate { get; set; }
        public IList<TotalClientBalancePdf> TotalClientBalancePdfs { get; set; }
    }
}
