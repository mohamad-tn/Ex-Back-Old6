using Abp.Events.Bus;
using Bwr.Exchange.Shared.Balances;

namespace Bwr.Exchange.Settings.Treasurys.Events
{
    public class AddTreasuryBalanceEventData : BalanceEventData
    {
        public AddTreasuryBalanceEventData(int currencyId) : base(currencyId)
        {

        }
    }
}
