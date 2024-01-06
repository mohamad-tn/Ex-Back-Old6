using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bwr.Exchange.Transfers.ExternalTransfers.Services
{
    public interface IExternalTreansferManager : IDomainService
    {
        Task<IList<ExtrenalTransfer>> GetAllAsync();
        IList<ExtrenalTransfer> GetAll();
        Task<ExtrenalTransfer> InsertAndGetAsync(ExtrenalTransfer extrenalTransfer);
        Task<ExtrenalTransfer> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
