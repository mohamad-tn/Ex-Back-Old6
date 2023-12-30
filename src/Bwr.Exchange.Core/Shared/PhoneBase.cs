using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Bwr.Exchange.Shared
{
    public class PhoneBase : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
