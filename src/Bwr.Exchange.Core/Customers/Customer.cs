using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;

namespace Bwr.Exchange.Customers
{
    public class Customer : FullAuditedEntity, IMayHaveTenant
    {
        public Customer()
        {
            Images = new List<CustomerImage>();
        }
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentificationNumber { get; set; }
        public virtual IList<CustomerImage> Images { get; set; }
    }
}
