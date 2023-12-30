using Abp.Domain.Entities;
using Bwr.Exchange.Shared;

namespace Bwr.Exchange.Customers
{
    public class CustomerImage : FileUploadBase, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
