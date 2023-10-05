﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Bwr.Exchange.TreasuryActions.Dto
{
    public class SearchTreasuryActionDto
    {
        public int Number { get; set; }
        public int ActionType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int MainAccount { get; set; }
        public int? CurrencyId { get; set; }
        public int? MainAccountCompanyId { get; set; }
        public int? MainAccountClientId { get; set; }
        public int? ExpenseId { get; set; }
        public int? IncomeId { get; set; }
        public int? IncomeTransferDetailId { get; set; }
    }
}
