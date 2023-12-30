using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Bwr.Exchange.Settings.Incomes
{
    public class Income : FullAuditedEntity, IMayHaveTenant
    {
        public Income(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
        public int? TenantId { get; set; }
    }
}
