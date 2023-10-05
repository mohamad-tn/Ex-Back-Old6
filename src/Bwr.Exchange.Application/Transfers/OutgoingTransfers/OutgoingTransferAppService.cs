using Bwr.Exchange.Customers;
using Bwr.Exchange.Customers.Dto;
using Bwr.Exchange.Customers.Services;
using Bwr.Exchange.Settings.Treasuries.Services;
using Bwr.Exchange.Shared.Dto;
using Bwr.Exchange.Transfers.OutgoingTransfers.Dto;
using Bwr.Exchange.Transfers.OutgoingTransfers.Services;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bwr.Exchange.Reflection.Extensions;
using System.Linq;
using System.Collections;
using Abp.UI;
using Microsoft.AspNetCore.Hosting;

namespace Bwr.Exchange.Transfers.OutgoingTransfers
{
    public class OutgoingTransferAppService : ExchangeAppServiceBase, IOutgoingTransferAppService
    {
        private readonly IOutgoingTransferManager _outgoingTransferManager;
        private readonly ICustomerManager _customerManager;
        private readonly ITreasuryManager _treasuryManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public OutgoingTransferAppService(
            OutgoingTransferManager outgoingTransferManager, 
            ICustomerManager customerManager,
            ITreasuryManager treasuryManager,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _outgoingTransferManager = outgoingTransferManager;
            _customerManager = customerManager;
            _treasuryManager = treasuryManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<OutgoingTransferDto> CreateAsync(OutgoingTransferDto input)
        {
            var treasury = await _treasuryManager.GetTreasuryAsync();
            var outgoingTransfer = ObjectMapper.Map<OutgoingTransfer>(input);

            var sender = await CreateOrUpdateCustomer(input.Sender);
            var beneficiary = await CreateOrUpdateCustomer(input.Beneficiary);

            outgoingTransfer.SenderId = sender?.Id;
            outgoingTransfer.BeneficiaryId = beneficiary?.Id;

            outgoingTransfer.TreasuryId = treasury.Id;

            var createdOutgoingTransfer = await _outgoingTransferManager.CreateAsync(outgoingTransfer);
            return ObjectMapper.Map<OutgoingTransferDto>(createdOutgoingTransfer);
        }

        public async Task<OutgoingTransferDto> UpdateAsync(OutgoingTransferDto input)
        {
            var outgoingTransfer = await _outgoingTransferManager.GetByIdAsync(input.Id);
            var date = DateTime.Parse(input.Date);
            date = new DateTime
                    (
                        date.Year,
                        date.Month,
                        date.Day,
                        outgoingTransfer.Date.Hour,
                        outgoingTransfer.Date.Minute,
                        outgoingTransfer.Date.Second
                    );
            var isDeleted = await _outgoingTransferManager.DeleteCashFlowAsync(outgoingTransfer);
            if (isDeleted)
            {
                ObjectMapper.Map<OutgoingTransferDto, OutgoingTransfer>(input, outgoingTransfer);

                outgoingTransfer.Date = date;
                var sender = await CreateOrUpdateCustomer(input.Sender);
                var beneficiary = await CreateOrUpdateCustomer(input.Beneficiary);

                outgoingTransfer.SenderId = sender?.Id;
                outgoingTransfer.BeneficiaryId = beneficiary?.Id;

                var updatedOutgoingTransfer = await _outgoingTransferManager.UpdateAsync(outgoingTransfer);
                return ObjectMapper.Map<OutgoingTransferDto>(updatedOutgoingTransfer);
            }
            else
            {
                throw new UserFriendlyException("حدث خطأ ما اثناء التعديل");
            }
        }

        public async Task DeleteAsync(int id)
        {
            var outgoingTransfer = await _outgoingTransferManager.GetByIdAsync(id);
            if(outgoingTransfer != null)
            {
                await _outgoingTransferManager.DeleteAsync(outgoingTransfer);
            }
        }

        public async Task<IList<OutgoingTransferDto>> Get(SearchOutgoingTransferInputDto input)
        {
            var dic = input.ToDictionary();
            var outgoingTransfers = await _outgoingTransferManager.GetAsync(dic);
            return ObjectMapper.Map<List<OutgoingTransferDto>>(outgoingTransfers);
        }

        public IList<ReadOutgoingTransferDto> GetForStatment(SearchOutgoingTransferInputDto input)
        {
            if (!string.IsNullOrEmpty(input.FromDate))
            {
                DateTime fromDate;
                DateTime.TryParse(input.FromDate, out fromDate);
                fromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 12, 0, 0);

                input.FromDate = fromDate.Date.ToString();
            }

            if (!string.IsNullOrEmpty(input.ToDate))
            {
                DateTime toDate;
                DateTime.TryParse(input.ToDate, out toDate);
                toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                input.ToDate = toDate.ToString();
            }

            var dic = input.ToDictionary();
            var outgoingTransfers = _outgoingTransferManager.Get(dic);
            return ObjectMapper.Map<List<ReadOutgoingTransferDto>>(outgoingTransfers);
        }

        public OutgoingTransferDto GetById(int id)
        {
            var outgoingTransfer = _outgoingTransferManager.GetById(id);
            return ObjectMapper.Map<OutgoingTransferDto>(outgoingTransfer);
        }

        public async Task<OutgoingTransferDto> GetForEditAsync(int id)
        {
            var transfer = await _outgoingTransferManager.GetByIdAsync(id);
            return ObjectMapper.Map<OutgoingTransferDto>(transfer);
        }

        [HttpPost]
        public ReadGrudDto GetForGrid([FromBody] DataManagerRequest dm)
        {
            var input = new SearchOutgoingTransferInputDto();

            if (dm.Where != null)
            {
                var numberFilter = GetWhereFilter(dm.Where, "number");
                if(numberFilter != null)
                {
                    input.Number = int.Parse(numberFilter.value.ToString());
                }
                else
                {
                    var fromDateFilter = GetWhereFilter(dm.Where, "fromDate");
                    if (fromDateFilter != null)
                    {
                        DateTime fromDate;
                        DateTime.TryParse(fromDateFilter.value.ToString(), out fromDate);
                        fromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 12, 0, 0);

                        input.FromDate = fromDate.Date.ToString();
                    }

                    var toDateFilter = GetWhereFilter(dm.Where, "toDate");
                    if (toDateFilter != null)
                    {
                        DateTime toDate;
                        DateTime.TryParse(toDateFilter.value.ToString(), out toDate);
                        toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                        input.ToDate = toDate.ToString();
                    }

                    var paymentTypeFilter = GetWhereFilter(dm.Where, "paymentType");
                    if (paymentTypeFilter != null)
                    {
                        input.PaymentType = paymentTypeFilter.value.ToString();
                    }

                    var clientFilter = GetWhereFilter(dm.Where, "clientId");
                    if (clientFilter != null)
                    {
                        input.ClientId = clientFilter.value.ToString();
                    }

                    var companyFilter = GetWhereFilter(dm.Where, "companyId");
                    if (companyFilter != null)
                    {
                        input.CompanyId = companyFilter.value.ToString();
                    }

                    var countryFilter = GetWhereFilter(dm.Where, "countryId");
                    if (countryFilter != null)
                    {
                        input.CountryId = countryFilter.value.ToString();
                    }

                    var beneficiaryFilter = GetWhereFilter(dm.Where, "beneficiary");
                    if (beneficiaryFilter != null)
                    {
                        input.Beneficiary = beneficiaryFilter.value.ToString();
                    }

                    var beneficiaryAddressFilter = GetWhereFilter(dm.Where, "beneficiaryAddress");
                    if (beneficiaryAddressFilter != null)
                    {
                        input.BeneficiaryAddress = beneficiaryAddressFilter.value.ToString();
                    }

                    var senderFilter = GetWhereFilter(dm.Where, "sender");
                    if (senderFilter != null)
                    {
                        input.Sender = senderFilter.value.ToString();
                    }
                }
            }
            else
            {
                var currentDate = DateTime.Now;
                var fromDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 12, 0, 0);
                input.FromDate = fromDate.ToString();

                var toDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 59);
                input.ToDate = toDate.ToString();
            }

            var dic = input.ToDictionary();
            var outgoinTransfers = _outgoingTransferManager.Get(dic);

            IEnumerable<ReadOutgoingTransferDto> data = ObjectMapper.Map<List<ReadOutgoingTransferDto>>(outgoinTransfers);
            var operations = new DataOperations();

            IEnumerable groupDs = new List<ReadOutgoingTransferDto>();
            if (dm.Group != null)
            {
                groupDs = operations.PerformSelect(data, dm.Group);
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

        private async Task<Customer> CreateOrUpdateCustomer(CustomerDto customerDto)
        {
            var customer = ObjectMapper.Map<Customer>(customerDto);
            return await _customerManager.CreateOrUpdateAsync(customer);
        }

        private WhereFilter GetWhereFilter(List<WhereFilter> filterOptions, string name)
        {
            var filter = filterOptions.FirstOrDefault(x => x.Field == name);
            if (filter != null)
                return filter;

            foreach (var option in filterOptions)
            {
                if(option.predicates != null)
                    return GetWhereFilter(option.predicates, name);
            }

            return null;
        }

        public int GetLastNumber()
        {
            return _outgoingTransferManager.GetLastNumber();
        }
    }
}
