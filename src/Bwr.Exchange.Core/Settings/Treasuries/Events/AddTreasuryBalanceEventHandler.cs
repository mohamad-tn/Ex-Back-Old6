using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using Bwr.Exchange.Settings.Treasuries;
using Bwr.Exchange.Settings.Treasuries.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Bwr.Exchange.Settings.Treasurys.Events
{
    public class AddTreasuryBalanceEventHandler : IAsyncEventHandler<AddTreasuryBalanceEventData>, ITransientDependency
    {
        private readonly ITreasuryManager _treasuryManager;
        private readonly ITreasuryBalanceManager _treasuryBalanceManager;

        public AddTreasuryBalanceEventHandler(
            ITreasuryManager treasuryManager, 
            ITreasuryBalanceManager treasuryBalanceManager)
        {
            _treasuryManager = treasuryManager;
            _treasuryBalanceManager = treasuryBalanceManager;
        }

        public async Task HandleEventAsync(AddTreasuryBalanceEventData eventData)
        {
            var treasuries = await _treasuryManager.GetAllAsync();   
            if (treasuries.Any())
            {
                foreach(var treasury in treasuries)
                {
                    var treasuryBalance = new TreasuryBalance(0, eventData.CurrencyId, treasury.Id);

                    await _treasuryBalanceManager.InsertAndGetAsync(treasuryBalance);
                }
                
            }
        }
    }
}
