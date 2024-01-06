using Bwr.Exchange.LinkTenantsCompanies.Dto;
using Bwr.Exchange.LinkTenantsCompanies.Sevices;
using Bwr.Exchange.Settings.Companies.Services;
using System.Threading.Tasks;

namespace Bwr.Exchange.LinkTenantsCompanies
{
    public class LinkTenantCompanyAppService : ExchangeAppServiceBase, ILinkTenantCompanyAppService
    {
        private readonly ILinkTenantCompanyManager _linkTenantCompanyManager;
        private readonly ICompanyManager _companyManager;

        public LinkTenantCompanyAppService(ILinkTenantCompanyManager linkTenantCompanyManager, ICompanyManager companyManager)
        {
            _linkTenantCompanyManager = linkTenantCompanyManager;
            _companyManager = companyManager;
        }

        public async Task DeleteAsync(int id)
        {
            var linkTenantCompany = await _linkTenantCompanyManager.GetByIdAsync(id);
            await _linkTenantCompanyManager.DeleteAsync(id);

            using (CurrentUnitOfWork.SetTenantId(linkTenantCompany.SecondTenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                var company = await _companyManager.GetByIdAsync(linkTenantCompany.CompanyId);
                company.TenantCompanyId = null;
                await _companyManager.UpdateAndGetAsync(company);
                CurrentUnitOfWork.SaveChanges();
            }
        }

        public LinkTenantCompanyDto GetByFirstTenantId(int firstTenantId)
        {
            var linkTenantCompany = _linkTenantCompanyManager.GetByFirstTenantId(firstTenantId);
            return ObjectMapper.Map<LinkTenantCompanyDto>(linkTenantCompany);
        }
    }
}
