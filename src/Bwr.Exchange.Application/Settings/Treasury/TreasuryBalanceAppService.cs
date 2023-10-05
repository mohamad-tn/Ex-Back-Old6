using Bwr.Exchange.Settings.Treasuries;
using Bwr.Exchange.Settings.Treasuries.Services;
using Bwr.Exchange.Settings.Treasury.Dto.TreasuryBalance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bwr.Exchange.Settings.Treasury
{
    public class TreasuryBalanceAppService : ExchangeAppServiceBase, ITreasuryBalanceAppService
    {
        private readonly TreasuryBalanceManager _treasuryBalanceManager;

        public TreasuryBalanceAppService(TreasuryBalanceManager treasuryBalanceManager)
        {
            _treasuryBalanceManager = treasuryBalanceManager;
        }

        public IList<TreasuryBalanceDto> GetAllWithDetails()
        {
            var treasuryBalances = _treasuryBalanceManager.GetAllWithDetails();
            return ObjectMapper.Map<List<TreasuryBalanceDto>>(treasuryBalances);
        }

        public async Task<TreasuryBalanceDto> InsertAndGetAsync(TreasuryBalanceDto input)
        {
            var treasuryBalance = ObjectMapper.Map<TreasuryBalance>(input);

            var createdTreasuryBalance = await _treasuryBalanceManager.InsertAndGetAsync(treasuryBalance);

            return ObjectMapper.Map<TreasuryBalanceDto>(createdTreasuryBalance);
        }

        public async Task<TreasuryBalanceDto> UpdateAndGetAsync(TreasuryBalanceDto input)
        {
            var treasuryBalance = await _treasuryBalanceManager.GetByIdAsync(input.Id);

            ObjectMapper.Map<TreasuryBalanceDto, TreasuryBalance>(input, treasuryBalance);

            var updatedTreasuryBalance = await _treasuryBalanceManager.UpdateAndGetAsync(treasuryBalance);

            return ObjectMapper.Map<TreasuryBalanceDto>(updatedTreasuryBalance);
        }
    }
}
