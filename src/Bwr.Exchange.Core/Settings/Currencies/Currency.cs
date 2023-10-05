using Abp.Domain.Entities.Auditing;

namespace Bwr.Exchange.Settings.Currencies
{
    public class Currency : FullAuditedEntity
    {
        public string Name { get; set; }
        public bool IsMainCurrency { get; set; }

    }
}
