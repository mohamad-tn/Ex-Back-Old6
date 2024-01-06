using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bwr.Exchange.Transfers.ExternalTransfers.Services
{
    public class ExternalTreansferManager : IExternalTreansferManager
    {
        private readonly IRepository<ExtrenalTransfer> _externalTrransferRepository;

        public ExternalTreansferManager(IRepository<ExtrenalTransfer> externalTrransferRepository)
        {
            _externalTrransferRepository = externalTrransferRepository;
        }

        public IList<ExtrenalTransfer> GetAll()
        {
            return _externalTrransferRepository.GetAll().ToList();
        }

        public async Task<IList<ExtrenalTransfer>> GetAllAsync()
        {
            return await _externalTrransferRepository.GetAllListAsync();
        }

        public async Task<ExtrenalTransfer> GetByIdAsync(int id)
        {
            return await _externalTrransferRepository.GetAsync(id);
        }

        public async Task<ExtrenalTransfer> InsertAndGetAsync(ExtrenalTransfer extrenalTransfer)
        {
            return await _externalTrransferRepository.InsertAsync(extrenalTransfer);
        }

        public async Task DeleteAsync(int id)
        {
            var externalTransfer = await GetByIdAsync(id);
            if(externalTransfer != null)
            await _externalTrransferRepository.DeleteAsync(externalTransfer);
        }
    }
}
