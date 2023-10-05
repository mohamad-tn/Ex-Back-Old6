using Abp.Domain.Entities.Auditing;

namespace Bwr.Exchange.Settings.Expenses
{
    public class Expense : FullAuditedEntity
    {
        public Expense(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
    }
}
