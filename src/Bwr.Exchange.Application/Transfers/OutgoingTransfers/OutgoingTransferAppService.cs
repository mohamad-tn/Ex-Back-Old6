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
using Abp.Events.Bus;
using Bwr.Exchange.Transfers.Customers.Events;
using Syncfusion.EJ2.Spreadsheet;
using Bwr.Exchange.CashFlows.ManagementStatement.Events;
using Abp.Runtime.Session;
using Bwr.Exchange.ExchangeCurrencies;
using Bwr.Exchange.Settings.Clients.Services;
using Bwr.Exchange.Settings.Companies.Services;
using Bwr.Exchange.Settings.Currencies.Services;

namespace Bwr.Exchange.Transfers.OutgoingTransfers
{
    public class OutgoingTransferAppService : ExchangeAppServiceBase, IOutgoingTransferAppService
    {
        private readonly IOutgoingTransferManager _outgoingTransferManager;
        private readonly ICustomerManager _customerManager;
        private readonly ITreasuryManager _treasuryManager;
        private readonly ICompanyManager _companyManager;
        private readonly IClientManager _clientManager;
        private readonly ICurrencyManager _currencyManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public OutgoingTransferAppService(
            OutgoingTransferManager outgoingTransferManager,
            ICustomerManager customerManager,
            ITreasuryManager treasuryManager,
            ICompanyManager companyManager,
            IClientManager clientManager,
            ICurrencyManager currencyManager,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _outgoingTransferManager = outgoingTransferManager;
            _customerManager = customerManager;
            _treasuryManager = treasuryManager;
            _companyManager = companyManager;
            _clientManager = clientManager;
            _currencyManager = currencyManager;
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
            string before = "";
            string after = "";

            var outgoingTransfer = await _outgoingTransferManager.GetByIdAsync(input.Id);

            #region Before & After
            if (outgoingTransfer.Note != input.Note)
            {
                before = L("Note") + " : " + outgoingTransfer.Note;
                after = L("Note") + " : " + input.Note;
            }

            if (outgoingTransfer.Number != input.Number)
            {
                before = before + " - " + L("Number") + " : " + outgoingTransfer.Number;
                after = after + " - " + L("Number") + " : " + input.Number;
            }

            if (outgoingTransfer.CurrencyId != input.CurrencyId)
            {
                before = before + " - " + L("Currency") + " : " + (outgoingTransfer.CurrencyId != null ? _currencyManager.GetCurrencyNameById(outgoingTransfer.CurrencyId) : " ");
                after = after + " - " + L("Currency") + " : " + (input.CurrencyId != null ? _currencyManager.GetCurrencyNameById(input.CurrencyId) : " ");
            }

            if (outgoingTransfer.BeneficiaryId != input.BeneficiaryId)
            {
                before = before + " - " + L("Beneficiary") + " : " + (outgoingTransfer.BeneficiaryId != null ? _customerManager.GetCustomerNameById((int)outgoingTransfer.BeneficiaryId) : " ");
                after = after + " - " + L("Beneficiary") + " : " + (input.BeneficiaryId != null ?_customerManager.GetCustomerNameById((int)input.BeneficiaryId) : " ");
            }

            if (outgoingTransfer.SenderId != input.SenderId)
            {
                before = before + " - " + L("Sender") + " : " + (outgoingTransfer.SenderId != null ? _customerManager.GetCustomerNameById((int)outgoingTransfer.SenderId) : " ");
                after = after + " - " + L("Sender") + " : " + (input.SenderId != null ? _customerManager.GetCustomerNameById((int)input.SenderId) : " ");
            }

            if (outgoingTransfer.Amount != input.Amount)
            {
                before = before + " - " + L("Amount") + " : " + outgoingTransfer.Amount;
                after = after + " - " + L("Amount") + " : " + input.Amount;
            }

            if (outgoingTransfer.ToCompanyId != input.ToCompanyId)
            {
                before = before + " - " + L("ToCompany") + " : " + (outgoingTransfer.ToCompanyId != null ? _companyManager.GetCompanyNameById((int)outgoingTransfer.ToCompanyId) : " ");
                after = after + " - " + L("ToCompany") + " : " + (input.ToCompanyId != null ? _companyManager.GetCompanyNameById((int)input.ToCompanyId) : " ");
            }

            if (outgoingTransfer.FromCompanyId != input.FromCompanyId)
            {
                before = before + " - " + L("FromCompany") + " : " + (outgoingTransfer.FromCompanyId != null ? _companyManager.GetCompanyNameById((int)outgoingTransfer.FromCompanyId) : " ");
                after = after + " - " + L("FromCompany") + " : " + (input.FromCompanyId != null ? _companyManager.GetCompanyNameById((int)input.FromCompanyId) : " ");
            }

            if ((int)outgoingTransfer.PaymentType != input.PaymentType)
            {
                before = before + " - " + L("PaymentType") + " : " + ((PaymentType)outgoingTransfer.PaymentType);
                after = after + " - " + L("PaymentType") + " : " + ((PaymentType)input.PaymentType);
            }

            if (outgoingTransfer.FromClientId != input.FromClientId)
            {
                before = before + " - " + L("FromClient") + " : " + (outgoingTransfer.FromClientId != null ? _clientManager.GetClientNameById((int)outgoingTransfer.FromClientId) : " ");
                after = after + " - " + L("FromClient") + " : " + (input.FromClientId != null ? _clientManager.GetClientNameById((int)input.FromClientId) : " ");
            }

            if (outgoingTransfer.ReceivedAmount != input.ReceivedAmount)
            {
                before = before + " - " + L("ReceivedAmount") + " : " + outgoingTransfer.ReceivedAmount;
                after = after + " - " + L("ReceivedAmount") + " : " + input.ReceivedAmount;
            }

            if (outgoingTransfer.InstrumentNo != input.InstrumentNo)
            {
                before = before + " - " + L("InstrumentNo") + " : " + outgoingTransfer.InstrumentNo;
                after = after + " - " + L("InstrumentNo") + " : " + input.InstrumentNo;
            }

            if (outgoingTransfer.Reason != input.Reason)
            {
                before = before + " - " + L("Reason") + " : " + outgoingTransfer.Reason;
                after = after + " - " + L("Reason") + " : " + input.Reason;
            }
            #endregion


            EventBus.Default.Trigger(
                new CreateManagementEventData(
                    0, outgoingTransfer.Amount, outgoingTransfer.Date, (int?)outgoingTransfer.PaymentType,
                    DateTime.Now, 0, outgoingTransfer.Number, null, null, before, after, null, null, null, null,
                    null, null, outgoingTransfer.Commission, null, null, outgoingTransfer.CurrencyId,
                    outgoingTransfer.FromClientId, AbpSession.GetUserId(), outgoingTransfer.FromCompanyId
                    , outgoingTransfer.SenderId, outgoingTransfer.BeneficiaryId, outgoingTransfer.ToCompanyId, null
                    )
                );


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

                EventBus.Default.Trigger(
                new CreateManagementEventData(
                    0,outgoingTransfer.Amount,outgoingTransfer.Date, (int?)outgoingTransfer.PaymentType,
                    DateTime.Now,1,outgoingTransfer.Number,null,null,null,null,null,null,null,null,
                    null,null,outgoingTransfer.Commission,null,null,outgoingTransfer.CurrencyId,
                    outgoingTransfer.FromClientId,AbpSession.GetUserId(),outgoingTransfer.FromCompanyId
                    ,outgoingTransfer.SenderId,outgoingTransfer.BeneficiaryId,outgoingTransfer.ToCompanyId,null
                    )
                );
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
