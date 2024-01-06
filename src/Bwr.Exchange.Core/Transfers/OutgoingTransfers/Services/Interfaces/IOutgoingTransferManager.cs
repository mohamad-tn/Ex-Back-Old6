using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bwr.Exchange.Transfers.OutgoingTransfers.Services
{
    public interface IOutgoingTransferManager : IDomainService
    {
        Task<OutgoingTransfer> CreateAsync(OutgoingTransfer input, int? currentTenantId);
        Task<OutgoingTransfer> UpdateAsync(OutgoingTransfer input);
        Task<OutgoingTransfer> DeleteAsync(OutgoingTransfer outgoingTransfer);
        Task<bool> DeleteCashFlowAsync(OutgoingTransfer input);
        Task<OutgoingTransfer> GetByIdAsync(int id);
        Task<IList<OutgoingTransfer>> GetAsync(Dictionary<string, object> dic);
        IList<OutgoingTransfer> Get(Dictionary<string, object> dic);
        OutgoingTransfer GetById(int id);
        Task SetAsCopied(List<int> ids);
        Task<List<NotCopiedForCompany>> GetNotCopiedCount();
        int GetLastNumber();
    }
}
