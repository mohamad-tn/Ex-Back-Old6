using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Handlers;
using Bwr.Exchange.Settings.Treasuries;
using System.Linq;
using System.Threading.Tasks;

namespace Bwr.Exchange.Settings.Treasurys.Events
{
    public class DeleteTreasuryBalanceEventHandler : IAsyncEventHandler<DeleteTreasuryBalanceEventData>, ITransientDependency
    {
        private readonly IRepository<TreasuryBalance> _treasuryBalanceRepository;

        public DeleteTreasuryBalanceEventHandler(IRepository<TreasuryBalance> treasuryBalanceRepository)
        {
            _treasuryBalanceRepository = treasuryBalanceRepository;
        }

        public async Task HandleEventAsync(DeleteTreasuryBalanceEventData eventData)
        {
            var treasuryBalances = await _treasuryBalanceRepository.GetAllListAsync(x => x.CurrencyId == eventData.CurrencyId);
            if (treasuryBalances.Any())
            {
                foreach (var treasuryBalance in treasuryBalances)
                {
                    await _treasuryBalanceRepository.DeleteAsync(treasuryBalance);
                }
            }
        }
    }
}
