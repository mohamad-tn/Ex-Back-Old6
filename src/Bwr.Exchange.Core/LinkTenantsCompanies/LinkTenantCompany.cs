using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Bwr.Exchange.LinkTenantsCompanies
{
    public class LinkTenantCompany : FullAuditedEntity, IMayHaveTenant
    {
        public LinkTenantCompany(int firstTenantId, int secondTenantId, int companyId)
        {
            FirstTenantId = firstTenantId;
            SecondTenantId = secondTenantId;
            CompanyId = companyId;
        }

        public int? TenantId { get; set; }
        public int FirstTenantId { get; set; }
        public int SecondTenantId { get; set; }
        public int CompanyId { get; set; }
    }
}
