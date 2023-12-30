using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bwr.Exchange.Settings.Countries
{
    public class Province: FullAuditedEntity, IMayHaveTenant
    {
        public string Name { get; set; }
        public int? TenantId { get; set; }
        #region Country
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
        #endregion 
    }
}
