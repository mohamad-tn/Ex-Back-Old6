using Abp.Domain.Repositories;
using Bwr.Exchange.Settings.Currencies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bwr.Exchange.Settings.Treasuries.Services
{
    public class TreasuryManager : ITreasuryManager
    {
        private readonly IRepository<Treasury> _treasuryRepository;
        //private readonly IRepository<TreasuryBalance> _treasuryBalanceRepository;
        private readonly IRepository<Currency> _currencyRepository;

        public TreasuryManager(
            IRepository<Treasury> treasuryRepository, 
            //IRepository<TreasuryBalance> treasuryBalanceRepository, 
            IRepository<Currency> currencyRepository)
        {
            _treasuryRepository = treasuryRepository;
            //_treasuryBalanceRepository = treasuryBalanceRepository;
            _currencyRepository = currencyRepository;
        }

        public async Task CreateMainTreasuryAsync()
        {
            var treasuryExist = _treasuryRepository.GetAll().Any();
            if (!treasuryExist)
            {
                var treasury = new Treasury();
                treasury.Name = "الصندوق الرئيسي";
                var createdTreasury = await _treasuryRepository.InsertAsync(treasury);

                //var currencies = await _currencyRepository.GetAllListAsync();

                //var mainTreasury = _treasuryRepository.FirstOrDefault(treasuryId);
                //foreach (var currency in currencies)
                //{
                //    var treasuryBalanceExist = _treasuryBalanceRepository.GetAll().Any(x => x.CurrencyId == currency.Id);
                //    if (!treasuryBalanceExist)
                //    {
                //        var treasuryBalance = new TreasuryBalance(0, currency.Id, mainTreasury.Id);
                //        await _treasuryBalanceRepository.InsertAsync(treasuryBalance);
                //    }
                //}
            }
            
        }

        public async Task CreateMainTreasuryForTenantAsync(int? tenantId)
        {
            var treasury = new Treasury();
            treasury.Name = "الصندوق الرئيسي";
            treasury.TenantId = tenantId;
            var createdTreasury = await _treasuryRepository.InsertAsync(treasury);
        }

        public async Task<IList<Treasury>> GetAllAsync()
        {
            return await _treasuryRepository.GetAllListAsync();
        }

        public async Task<Treasury> GetTreasuryAsync()
        {
            var treasuries = await _treasuryRepository.GetAllListAsync();
            return treasuries != null ? treasuries.FirstOrDefault() : null;
        }
    }
}
