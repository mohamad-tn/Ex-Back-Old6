using Abp.Domain.Entities.Auditing;

namespace Bwr.Exchange.Settings.Incomes
{
    public class Income : FullAuditedEntity
    {
        public Income(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
    }
}
