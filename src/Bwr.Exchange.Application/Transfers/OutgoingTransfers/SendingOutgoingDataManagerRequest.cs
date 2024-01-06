using Bwr.Exchange.Shared.DataManagerRequests;
using Syncfusion.EJ2.Base;

namespace Bwr.Exchange.Transfers.OutgoingTransfers
{
    public class SendingOutgoingDataManagerRequest : BWireDataManagerRequest
    {
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string companyId { get; set; }
        public int currencyId { get; set; }
    }
}
