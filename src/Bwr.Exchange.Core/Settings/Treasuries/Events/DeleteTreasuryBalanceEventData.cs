using Abp.Events.Bus;
using Bwr.Exchange.Shared.Balances;

namespace Bwr.Exchange.Settings.Treasurys.Events
{
    public class DeleteTreasuryBalanceEventData : BalanceEventData
    {
        public DeleteTreasuryBalanceEventData(int currencyId) : base(currencyId)
        {

        }
    }
}
