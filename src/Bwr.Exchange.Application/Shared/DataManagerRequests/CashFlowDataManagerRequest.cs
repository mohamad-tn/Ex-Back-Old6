﻿using Syncfusion.EJ2.Base;

namespace Bwr.Exchange.Shared.DataManagerRequests
{
    public class CashFlowDataManagerRequest: DataManagerRequest
    {
        public int id { get; set; }
        public int currencyId { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }
}
