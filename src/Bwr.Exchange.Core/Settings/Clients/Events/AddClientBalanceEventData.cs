using Bwr.Exchange.Shared.Balances;

namespace Bwr.Exchange.Settings.Clients.Events
{
    public class AddClientBalanceEventData : BalanceEventData
    {
        public AddClientBalanceEventData(int currencyId): base(currencyId)
        {
        }

    }
}
