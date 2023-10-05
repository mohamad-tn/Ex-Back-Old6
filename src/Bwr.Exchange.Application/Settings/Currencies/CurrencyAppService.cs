﻿using Abp.Threading;
using Abp.UI;
using Bwr.Exchange.Settings.Currencies.Dto;
using Bwr.Exchange.Settings.Currencies.Services;
using Bwr.Exchange.Shared.Dto;
using Bwr.Exchange.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bwr.Exchange.Settings.Currencies
{
    public class CurrencyAppService : ExchangeAppServiceBase, ICurrencyAppService
    {
        private readonly ICurrencyManager _currencyManager;
        public CurrencyAppService(CurrencyManager countryManager)
        {
            _currencyManager = countryManager;
        }

        public async Task<IList<CurrencyDto>> GetAllAsync()
        {
            var countries = await _currencyManager.GetAllAsync();

            return ObjectMapper.Map<List<CurrencyDto>>(countries);
        }
        [HttpPost]
        public ReadGrudDto GetForGrid([FromBody] DataManagerRequest dm)
        {
            var data = _currencyManager.GetAll();
            IEnumerable<ReadCurrencyDto> countries = ObjectMapper.Map<List<ReadCurrencyDto>>(data);

            var operations = new DataOperations();
            if (dm.Where != null && dm.Where.Count > 0)
            {
                countries = operations.PerformFiltering(countries, dm.Where, "and");
            }

            if (dm.Sorted != null && dm.Sorted.Count > 0)
            {
                countries = operations.PerformSorting(countries, dm.Sorted);
            }

            IEnumerable groupDs = new List<ReadCurrencyDto>();
            if (dm.Group != null)
            {
                groupDs = operations.PerformSelect(countries, dm.Group);
            }

            var count = countries.Count();

            if (dm.Skip != 0)
            {
                countries = operations.PerformSkip(countries, dm.Skip);
            }

            if (dm.Take != 0)
            {
                countries = operations.PerformTake(countries, dm.Take);
            }

            return new ReadGrudDto() { result = countries, count = count, groupDs = groupDs };
        }
        public UpdateCurrencyDto GetForEdit(int id)
        {
            var country = AsyncHelper.RunSync(() => _currencyManager.GetByIdAsync(id));
            return ObjectMapper.Map<UpdateCurrencyDto>(country);
        }
        public async Task<CurrencyDto> CreateAsync(CreateCurrencyDto input)
        {
            CheckBeforeCreate(input);

            var country = ObjectMapper.Map<Currency>(input);

            var createdCurrency = await _currencyManager.InsertAndGetAsync(country);

            return ObjectMapper.Map<CurrencyDto>(createdCurrency);
        }
        public async Task<CurrencyDto> UpdateAsync(UpdateCurrencyDto input)
        {
            CheckBeforeUpdate(input);

            var country = await _currencyManager.GetByIdAsync(input.Id);

            ObjectMapper.Map<UpdateCurrencyDto, Currency>(input, country);

            var updatedCurrency = await _currencyManager.UpdateAndGetAsync(country);

            return ObjectMapper.Map<CurrencyDto>(updatedCurrency);
        }
        public async Task DeleteAsync(int id)
        {
            await _currencyManager.DeleteAsync(id);
        }

        #region Helper methods
        private void CheckBeforeCreate(CreateCurrencyDto input)
        {
            var validationResultMessage = string.Empty;

            if (_currencyManager.CheckIfNameAlreadyExist(input.Name))
            {
                validationResultMessage = L(ValidationResultMessage.NameAleadyExist);
            }

            if (!string.IsNullOrEmpty(validationResultMessage))
                throw new UserFriendlyException(validationResultMessage);
        }
        private void CheckBeforeUpdate(UpdateCurrencyDto input)
        {
            var validationResultMessage = string.Empty;

            if (_currencyManager.CheckIfNameAlreadyExist(input.Id, input.Name))
            {
                validationResultMessage = L(ValidationResultMessage.NameAleadyExist);
            }

            if (!string.IsNullOrEmpty(validationResultMessage))
                throw new UserFriendlyException(validationResultMessage);
        }

        #endregion
    }
}
