﻿using Abp.Events.Bus;
using Bwr.Exchange.Shared.Balances;

namespace Bwr.Exchange.Settings.Companys.Events
{
    public class AddCompanyBalanceEventData : BalanceEventData
    {
        public AddCompanyBalanceEventData(int currencyId) : base(currencyId)
        {
        }

    }
}