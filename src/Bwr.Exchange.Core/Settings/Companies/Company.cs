using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;

namespace Bwr.Exchange.Settings.Companies
{
    public class Company : FullAuditedEntity
    {
        public Company()
        {
            CompanyBalances = new List<CompanyBalance>();
        }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public virtual IList<CompanyBalance> CompanyBalances { get; set; }
    }
}
