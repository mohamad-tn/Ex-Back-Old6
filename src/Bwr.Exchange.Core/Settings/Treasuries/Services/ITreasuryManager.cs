using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bwr.Exchange.Settings.Treasuries.Services
{
    public interface ITreasuryManager : IDomainService
    {
        Task<Treasury> GetTreasuryAsync();
        Task<IList<Treasury>> GetAllAsync();
        Task CreateMainTreasuryAsync();
        Task CreateMainTreasuryForTenantAsync(int? tenantId);
    }
}
