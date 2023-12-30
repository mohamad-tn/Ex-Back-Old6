using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Bwr.Exchange.Settings.Expenses
{
    public class Expense : FullAuditedEntity, IMayHaveTenant
    {
        public Expense(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
        public int? TenantId { get; set; }
    }
}
