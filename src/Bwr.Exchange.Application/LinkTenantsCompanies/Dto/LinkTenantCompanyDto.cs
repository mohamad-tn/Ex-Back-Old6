using Abp.Application.Services.Dto;

namespace Bwr.Exchange.LinkTenantsCompanies.Dto
{
    public class LinkTenantCompanyDto: EntityDto
    {
        public int FirstTenantId { get; set; }
        public int SecondTenantId { get; set; }
        public int CompanyId { get; set; }
    }
}
