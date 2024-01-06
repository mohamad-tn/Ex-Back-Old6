using Bwr.Exchange.Settings.Treasuries.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bwr.Exchange.Settings.Treasury
{
    public class TreasuryAppService : ExchangeAppServiceBase, ITreasuryAppService
    {
        private readonly ITreasuryManager _treasuryManager;

        public TreasuryAppService(ITreasuryManager treasuryManager)
        {
            _treasuryManager = treasuryManager;
        }

        public async Task CreateMainTreasuryAsync()
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                await _treasuryManager.CreateMainTreasuryAsync();
            }
        }
    }
}
