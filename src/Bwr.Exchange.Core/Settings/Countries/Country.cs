using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;

namespace Bwr.Exchange.Settings.Countries
{
    public class Country : FullAuditedEntity, IMayHaveTenant
    {
        public Country()
        {
            Provinces = new List<Province>();
        }
        public string Name { get; set; }
        public int? TenantId { get; set; }
        public virtual IList<Province> Provinces { get; set; }
    }
}
