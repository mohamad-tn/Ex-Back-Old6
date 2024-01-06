using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bwr.Exchange.LinkTenantsCompanies.Sevices
{
    public class LinkTenantCompanyManager : ILinkTenantCompanyManager
    {
        private readonly IRepository<LinkTenantCompany> _linkTenantCompanyRepository;

        public LinkTenantCompanyManager(IRepository<LinkTenantCompany> linkTenantCompanyRepository)
        {
            _linkTenantCompanyRepository = linkTenantCompanyRepository;
        }

        public async Task DeleteAsync(int id)
        {
            await _linkTenantCompanyRepository.DeleteAsync(id);
        }

        public IList<LinkTenantCompany> GetAll()
        {
            var linkTenantCompanies = _linkTenantCompanyRepository.GetAll();
            return linkTenantCompanies.ToList();
        }

        public LinkTenantCompany GetByFirstTenantId(int id)
        {
            var linkTenantCompany = _linkTenantCompanyRepository.GetAll()
                .FirstOrDefault(x => x.FirstTenantId == id);
            return linkTenantCompany;
        }

        public async Task<LinkTenantCompany> GetByIdAsync(int id)
        {
            var linkTenantCompany = await _linkTenantCompanyRepository.GetAsync(id);
            return linkTenantCompany;
        }

        public async Task<LinkTenantCompany> InsertAsync(LinkTenantCompany linkTenantCompany)
        {
            var createdLinkTenantCompanyId = await _linkTenantCompanyRepository.InsertAsync(linkTenantCompany);
            return createdLinkTenantCompanyId;
        }

        public async Task<LinkTenantCompany> UpdateAndGetAsync(LinkTenantCompany linkTenantCompany)
        {
            var updatedLinkTenantCompany = await _linkTenantCompanyRepository.UpdateAsync(linkTenantCompany);
            return updatedLinkTenantCompany;
        }
    }
}
