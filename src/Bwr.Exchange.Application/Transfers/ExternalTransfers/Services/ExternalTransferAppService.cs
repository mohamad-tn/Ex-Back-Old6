using Bwr.Exchange.Transfers.ExternalTransfers.Dto;
using System.Collections.Generic;

namespace Bwr.Exchange.Transfers.ExternalTransfers.Services
{
    public class ExternalTransferAppService : ExchangeAppServiceBase, IExternalTransferAppService
    {
        private readonly IExternalTreansferManager _externalTransferManager;

        public ExternalTransferAppService(IExternalTreansferManager externalTransferManager)
        {
            _externalTransferManager = externalTransferManager;
        }

        public IList<ExternalTransferDto> GetAll()
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                var externalTransfers = _externalTransferManager.GetAll();
                return ObjectMapper.Map<List<ExternalTransferDto>>(externalTransfers);
            }
        }
    }
}
