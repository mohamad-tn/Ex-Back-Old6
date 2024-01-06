using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bwr.Exchange.LinkTenantsCompanies.Sevices
{
    public interface ILinkTenantCompanyManager : IDomainService
    {
        IList<LinkTenantCompany> GetAll();
        Task<LinkTenantCompany> GetByIdAsync(int id);
        LinkTenantCompany GetByFirstTenantId(int id);
        Task<LinkTenantCompany> InsertAsync(LinkTenantCompany linkTenantCompany);
        Task<LinkTenantCompany> UpdateAndGetAsync(LinkTenantCompany linkTenantCompany);
        Task DeleteAsync(int id);
    }
}
