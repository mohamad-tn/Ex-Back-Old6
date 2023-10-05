﻿using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Bwr.Exchange.Settings.Clients.Events;
using Bwr.Exchange.Settings.Companys.Events;
using Bwr.Exchange.Settings.Currencies.Events;
using Bwr.Exchange.Settings.Treasurys.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bwr.Exchange.Settings.Currencies.Services
{
    public class CurrencyManager : ICurrencyManager
    {
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public CurrencyManager(
            IRepository<Currency> currencyRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _currencyRepository = currencyRepository;
            _unitOfWorkManager = unitOfWorkManager;
            
        }

        public bool CheckIfNameAlreadyExist(string name)
        {
            var currency = _currencyRepository.FirstOrDefault(x => x.Name.Trim().Equals(name.Trim()));
            return currency != null ? true : false;
        }

        public bool CheckIfNameAlreadyExist(int id, string name)
        {
            var currency = _currencyRepository.FirstOrDefault(x => x.Id != id && x.Name.Trim().Equals(name.Trim()));
            return currency != null ? true : false;
        }

        public async Task DeleteAsync(int id)
        {
            var currency = await GetByIdAsync(id);
            if (currency != null)
            {
                EventBus.Default.Trigger(new DeleteClientBalanceEventData(currency.Id));
                EventBus.Default.Trigger(new DeleteCompanyBalanceEventData(currency.Id));
                EventBus.Default.Trigger(new DeleteTreasuryBalanceEventData(currency.Id));
                await _currencyRepository.DeleteAsync(currency);
            }
        }

        public IList<Currency> GetAll()
        {
            return _currencyRepository.GetAll().ToList();
        }

        public async Task<IList<Currency>> GetAllAsync()
        {
            return await _currencyRepository.GetAllListAsync();
        }

        public async Task<Currency> GetByIdAsync(int id)
        {
            return await _currencyRepository.GetAsync(id);
        }

        public async Task<Currency> InsertAndGetAsync(Currency currency)
        {
            int currencyId = 0;
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                if(currency.IsMainCurrency)
                {
                    await CheckAnotherMainCurrency(currency);
                }
                currencyId = await _currencyRepository.InsertAndGetIdAsync(currency);

                await EventBus.Default.TriggerAsync(new AddClientBalanceEventData(currencyId));
                await EventBus.Default.TriggerAsync(new AddCompanyBalanceEventData(currencyId));
                await EventBus.Default.TriggerAsync(new AddTreasuryBalanceEventData(currencyId));

                await _unitOfWorkManager.Current.SaveChangesAsync();  
                await unitOfWork.CompleteAsync();
            }
            return await _currencyRepository.FirstOrDefaultAsync(currencyId);
        }

        private async Task CheckAnotherMainCurrency(Currency currency)
        {
            var anotherMainCurrency = await _currencyRepository
                .FirstOrDefaultAsync(x => x.IsMainCurrency == true && x.Id != currency.Id);
            if (anotherMainCurrency != null)
            {
                anotherMainCurrency.IsMainCurrency = false;
                await _currencyRepository.UpdateAsync(anotherMainCurrency);
            }
        }

        public async Task<Currency> UpdateAndGetAsync(Currency currency)
        {
            if (currency.IsMainCurrency)
            {
                await CheckAnotherMainCurrency(currency);
            }
            return await _currencyRepository.UpdateAsync(currency);
        }


    }
}
