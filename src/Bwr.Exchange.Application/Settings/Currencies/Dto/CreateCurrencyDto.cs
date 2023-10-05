using System.Collections.Generic;

namespace Bwr.Exchange.Settings.Currencies.Dto
{
    public class CreateCurrencyDto
    {
        public string Name { get; set; }
        public bool IsMainCurrency { get; set; }
    }
}
