using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;

namespace Bwr.Exchange.Settings.Companies
{
    public class Company : FullAuditedEntity, IMayHaveTenant
    {
        public Company()
        {
            CompanyBalances = new List<CompanyBalance>();
        }
        public int? TenantCompanyId { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public virtual IList<CompanyBalance> CompanyBalances { get; set; }
    }
}
