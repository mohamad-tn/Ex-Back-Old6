using AutoMapper;
using Bwr.Exchange.LinkTenantsCompanies.Dto;

namespace Bwr.Exchange.LinkTenantsCompanies.Map
{
    public class LinkTenantCompanyMapProfile : Profile
    {
        public LinkTenantCompanyMapProfile()
        {
            CreateMap<LinkTenantCompany, LinkTenantCompanyDto>();
        }
    }
}
