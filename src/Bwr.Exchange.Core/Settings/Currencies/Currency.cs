using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Bwr.Exchange.Settings.Currencies
{
    public class Currency : FullAuditedEntity, IMayHaveTenant
    {
        public string Name { get; set; }
        public bool IsMainCurrency { get; set; }
        public int? TenantId { get; set; }
    }
}
