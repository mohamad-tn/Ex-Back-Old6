using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Bwr.Exchange.CashFlows.CashFlowMatchings
{
    public class CashFlowMatching : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string MatchingBy{ get; set; }
        public string MatchingWith { get; set; }
        public string Description { get; set; }
    }
}
