using Abp.Application.Services;
using Bwr.Exchange.Transfers.ExternalTransfers.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bwr.Exchange.Transfers.ExternalTransfers.Services
{
    public interface IExternalTransferAppService : IApplicationService
    {
        IList<ExternalTransferDto> GetAll();
        Task AcceptExternalTransferAsync(int id);
    }
}
