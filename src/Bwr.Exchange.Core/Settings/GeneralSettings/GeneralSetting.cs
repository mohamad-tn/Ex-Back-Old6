using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Bwr.Exchange.Settings.GeneralSettings
{
    public class GeneralSetting : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string CompanyName { get; set; }
        public string DbSuffex { get; set; }
        public string EditPassword { get; set; }

    }
}
