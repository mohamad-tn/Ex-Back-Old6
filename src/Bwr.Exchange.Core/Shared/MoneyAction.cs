using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;

namespace Bwr.Exchange.Shared
{
    public class MoneyAction : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
    }
}
