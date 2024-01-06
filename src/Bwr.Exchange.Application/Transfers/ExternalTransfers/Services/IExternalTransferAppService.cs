using Abp.Application.Services;
using Bwr.Exchange.Transfers.ExternalTransfers.Dto;
using System.Collections.Generic;

namespace Bwr.Exchange.Transfers.ExternalTransfers.Services
{
    public interface IExternalTransferAppService : IApplicationService
    {
        IList<ExternalTransferDto> GetAll();
    }
}
