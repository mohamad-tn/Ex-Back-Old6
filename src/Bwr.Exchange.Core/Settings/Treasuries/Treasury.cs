using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;

namespace Bwr.Exchange.Settings.Treasuries
{
    public class Treasury : FullAuditedEntity, IMayHaveTenant
    {
        public Treasury()
        {
            TreasuryBalances = new List<TreasuryBalance>();
        }
        public string Name { get; set; }
        public int? TenantId { get; set; }
        public IList<TreasuryBalance> TreasuryBalances { get; set; }
    }
}
