using Abp.UI;
using Bwr.Exchange.CashFlows.Shared.Dto;
using Bwr.Exchange.Customers;
using Bwr.Exchange.Customers.Dto;
using Bwr.Exchange.Settings.Clients.Dto;
using Bwr.Exchange.Settings.Companies.Dto;
using Bwr.Exchange.Settings.Currencies.Dto;
using Bwr.Exchange.Settings.Currencies.Services;
using Bwr.Exchange.Shared.Dto;
using Bwr.Exchange.Transfers.IncomeTransfers.Dto;
using Bwr.Exchange.Transfers.IncomeTransfers.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bwr.Exchange.Transfers.IncomeTransfers
{
    public class IncomeTransferDetailAppService : ExchangeAppServiceBase, IIncomeTransferDetailAppService
    {
        private readonly IIncomeTransferDetailManager _incomeTransferDetailManager;
        private readonly ICustomerAppService _customerAppService;
        private readonly ICurrencyManager _currencyManager;

        public IncomeTransferDetailAppService(
            IIncomeTransferDetailManager incomeTransferDetailManager, 
            ICustomerAppService customerAppService, 
            ICurrencyManager currencyManager)
        {
            _incomeTransferDetailManager = incomeTransferDetailManager;
            _customerAppService = customerAppService;
            _currencyManager = currencyManager;
        }

        public async Task<IncomeTransferDetailDto> ChangeStatusAsync(IncomeTransferDetailChangeStatusInput input)
        {
            var updatedIncomeTransferDetail = await _incomeTransferDetailManager.ChangeStatusAsync(input.Id, input.Status);
            return ObjectMapper.Map<IncomeTransferDetailDto>(updatedIncomeTransferDetail);
        }

        public IList<IncomeTransferDetailDto> GetAllDirectTransfers()
        {
            var transfers = _incomeTransferDetailManager.GetAllDirectTransfers().ToList();
            return ObjectMapper.Map<List<IncomeTransferDetailDto>>(transfers);
        }

        [HttpPost]
        public ReadGrudDto GetDirectTransferForGrid([FromBody] DataManagerRequest dm)
        {
            int currencyId = 0;
            DateTime fromDate = DateTime.Now, toDate = DateTime.Now;
            if (dm.Where != null)
            {
                var currencyFilter = GetWhereFilter(dm.Where, "currencyId");
                if (currencyFilter != null)
                {
                    int.TryParse(currencyFilter.value.ToString(), out currencyId);
                }

                var fromDateFilter = GetWhereFilter(dm.Where, "fromDate");
                if (fromDateFilter != null)
                {
                    DateTime.TryParse(fromDateFilter.value.ToString(), out fromDate);
                }

                var toDateFilter = GetWhereFilter(dm.Where, "toDate");
                if (toDateFilter != null)
                {
                    DateTime.TryParse(toDateFilter.value.ToString(), out toDate);
                }
            }

            fromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0);
            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);

            IEnumerable<IncomeTransferDetail> directTransfers = _incomeTransferDetailManager.GetNotReceived()
                .Where(x => x.IncomeTransfer.Date >= fromDate && x.IncomeTransfer.Date <= toDate).ToList();

            if(currencyId > 0)
            {
                directTransfers = directTransfers.Where(x => x.CurrencyId == currencyId);
            }

            directTransfers = directTransfers.ToList();
            IEnumerable<ReadNoneReceivedTransferDto> data = ObjectMapper.Map<List<ReadNoneReceivedTransferDto>>(directTransfers);

            var operations = new DataOperations();

            IEnumerable groupDs = new List<ReadNoneReceivedTransferDto>();
            if (dm.Group != null)
            {
                groupDs = operations.PerformSelect(data, dm.Group);
            }

            if (dm.Search != null)
            {
                data = operations.PerformSearching(data, dm.Search);
            }

            var count = data.Count();

            if (dm.Skip != 0)
            {
                data = operations.PerformSkip(data, dm.Skip);
            }

            if (dm.Take != 0)
            {
                data = operations.PerformTake(data, dm.Take);
            }

            return new ReadGrudDto() { result = data, count = count, groupDs = groupDs };
        }

        public async Task<IList<FileUploadDto>> GetDirectTransferImagesAsync(int incomeTransferDetailId)
        {
            IList<FileUploadDto> images = new List<FileUploadDto>();
            var incomeTransferDetail = await _incomeTransferDetailManager.GetByIdAsync(incomeTransferDetailId);
            if(incomeTransferDetail != null && incomeTransferDetail.BeneficiaryId != null)
            {
                images = await _customerAppService.GetCustomerImagesAsync(incomeTransferDetail.BeneficiaryId.Value);
            }
            return images;
        }

        public IList<IncomeTransferDetailDto> GetNotReceived(int currencyId)
        {
            var transfers = _incomeTransferDetailManager.GetNotReceived().Where(x=>x.CurrencyId == currencyId).ToList();
            return ObjectMapper.Map<List<IncomeTransferDetailDto>>(transfers);
        }

        public IList<IncomeTransferDetailDto> GetAllNotReceived()
        {
            var transfers = _incomeTransferDetailManager.GetNotReceived().ToList();
            return ObjectMapper.Map<List<IncomeTransferDetailDto>>(transfers);
        }

        private WhereFilter GetWhereFilter(List<WhereFilter> filterOptions, string name)
        {
            var filter = filterOptions.FirstOrDefault(x => x.Field == name);
            if (filter != null)
                return filter;

            foreach (var option in filterOptions)
            {
                return GetWhereFilter(option.predicates, name);
            }

            return null;
        }

        public IList<SummaryCashFlowDto> Summary(string date)
        {
            var toDate = DateTime.Now;
            if (!string.IsNullOrEmpty(date))
            {
                toDate = DateTime.Parse(date);
            }
            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);

            var dtos = new List<SummaryCashFlowDto>();
            var currencies = _currencyManager.GetAll();

            var notReceivedTransfer = _incomeTransferDetailManager.GetNotReceivedToDate(toDate);
            foreach (var currency in currencies)
            {
                var dto = new SummaryCashFlowDto()
                {
                    Currency = ObjectMapper.Map<CurrencyDto>(currency),
                };

                var result = notReceivedTransfer.Where(x => x.CurrencyId == currency.Id);
                if (result.Any())
                {
                    dto.TotalBalance = result.Sum(x => x.Amount);
                }

                dtos.Add(dto);
            }

            return dtos;
        }
    }
}
