using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;

namespace Bwr.Exchange.Settings.Treasuries
{
    public class Treasury : FullAuditedEntity
    {
        public Treasury()
        {
            TreasuryBalances = new List<TreasuryBalance>();
        }
        public string Name { get; set; }

        public IList<TreasuryBalance> TreasuryBalances { get; set; }
    }
}
