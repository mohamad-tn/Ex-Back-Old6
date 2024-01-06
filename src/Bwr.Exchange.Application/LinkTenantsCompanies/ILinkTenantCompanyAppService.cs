using Abp.Application.Services;
using Bwr.Exchange.LinkTenantsCompanies.Dto;
using System.Threading.Tasks;

namespace Bwr.Exchange.LinkTenantsCompanies
{
    public interface ILinkTenantCompanyAppService : IApplicationService
    {
        LinkTenantCompanyDto GetByFirstTenantId(int firstTenantId);
        Task DeleteAsync(int id);
    }
}
