using Bwr.Exchange.Settings.Clients;
using Bwr.Exchange.Settings.Companies;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bwr.Exchange.Transfers.ExternalTransfers
{
    public class ExtrenalTransfer : Transfer
    {
        public DateTime Date { get; set; }
        public string Note { get; set; }

        #region Company
        public int? FromCompanyId { get; set; }
        [ForeignKey("FromCompanyId")]
        public virtual Company FromCompany { get; set; }
        public int? ToCompanyId { get; set; }
        [ForeignKey("ToCompanyId")]
        public virtual Company ToCompany { get; set; }
        #endregion

        #region Client
        public int? FromClientId { get; set; }
        [ForeignKey("FromClientId")]
        public virtual Client FromClient { get; set; }
        #endregion
    }
}
