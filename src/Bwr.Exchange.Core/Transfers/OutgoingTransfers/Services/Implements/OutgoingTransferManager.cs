using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.UI;
using Bwr.Exchange.Settings.Companies;
using Bwr.Exchange.Settings.Companies.Services;
using Bwr.Exchange.Settings.Currencies;
using Bwr.Exchange.Settings.Currencies.Services;
using Bwr.Exchange.Transfers.OutgoingTransfers.Events;
using Bwr.Exchange.Transfers.OutgoingTransfers.Factories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bwr.Exchange.Transfers.OutgoingTransfers.Services
{
    public class OutgoingTransferManager : IOutgoingTransferManager
    {
        private readonly IRepository<OutgoingTransfer> _outgoingTransferRepository;
        private readonly ICompanyManager _companyManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IOutgoingTransferFactory _outgoingTransferFactory;
        private readonly ICurrencyManager _currencyManager;
        //public IEventBus EventBus { get; set; }

        public OutgoingTransferManager(
            IRepository<OutgoingTransfer> outgoingTransferRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IOutgoingTransferFactory outgoingTransferFactory,
            ICompanyManager companyManager,
            ICurrencyManager currencyManager)
        {
            _outgoingTransferRepository = outgoingTransferRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _outgoingTransferFactory = outgoingTransferFactory;
            //EventBus = NullEventBus.Instance;
            _companyManager = companyManager;
            _currencyManager = currencyManager;
        }

        //public async Task<OutgoingTransfer> CreateAsync(OutgoingTransfer input)
        //{
        //    //Date and time
        //    var currentDate = DateTime.Now;
        //    input.Date = new DateTime(
        //        input.Date.Year,
        //        input.Date.Month,
        //        input.Date.Day,
        //        currentDate.Hour,
        //        currentDate.Minute,
        //        currentDate.Second
        //        );

        //    var id = await _outgoingTransferRepository.InsertAndGetIdAsync(input);
        //    var createdOutgoingTransfer = GetByIdWithDetail(id);

        //    IOutgoingTransferDomainService service = _outgoingTransferFactory.CreateService(input);
        //    await service.CreateCashFlowAsync(createdOutgoingTransfer);

        //    return createdOutgoingTransfer;
        //}

        public async Task<OutgoingTransfer> GetByIdAsync(int id)
        {
            return await _outgoingTransferRepository.FirstOrDefaultAsync(id);
        }

        public OutgoingTransfer GetById(int id)
        {
            return _outgoingTransferRepository.GetAllIncluding(
                b => b.Beneficiary,
                s => s.Sender
                ).Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task<IList<OutgoingTransfer>> GetAsync(Dictionary<string, object> dic)
        {
            IEnumerable<OutgoingTransfer> outgoingTransfers = await _outgoingTransferRepository.GetAllListAsync(x =>
              x.Date >= DateTime.Parse(dic["FromDate"].ToString()) &&
              x.Date >= DateTime.Parse(dic["ToDate"].ToString()));

            if (outgoingTransfers != null && outgoingTransfers.Any())
            {
                if (dic["PaymentType"] != null)
                {
                    outgoingTransfers = outgoingTransfers
                        .Where(x => x.PaymentType == (PaymentType)int.Parse(dic["PaymantType"].ToString()));
                }

                if (dic["ClientId"] != null)
                {
                    outgoingTransfers = outgoingTransfers
                        .Where(x => x.FromClientId == int.Parse(dic["ClientId"].ToString()));
                }
            }

            return outgoingTransfers.ToList();
        }

        public IList<OutgoingTransfer> Get(Dictionary<string, object> dic)
        {
            IEnumerable<OutgoingTransfer> outgoingTransfers = GetAllWithDetails();

            if (outgoingTransfers != null && outgoingTransfers.Any())
            {
                int number = 0;
                int.TryParse(dic["Number"].ToString(), out number);
                if (number != 0)
                {
                    outgoingTransfers = outgoingTransfers
                        .Where(x => x.Number == int.Parse(dic["Number"].ToString()));
                }
                else
                {
                    if (dic["FromDate"] != null && dic["ToDate"] != null)
                    {
                        //outgoingTransfers = outgoingTransfers.Where(x =>
                        //  x.Date >= DateTime.Parse(dic["FromDate"].ToString()) &&
                        //  x.Date <= DateTime.Parse(dic["ToDate"].ToString())).ToList();

                        outgoingTransfers = outgoingTransfers.Where(x =>
                          x.Date >= DateTime.Parse(DateTime.Parse(dic["FromDate"].ToString()).ToString("dd/MM/yyyy")) &&
                          x.Date <= DateTime.Parse(DateTime.Parse(dic["ToDate"].ToString()).ToString("dd/MM/yyyy"))).ToList();

                    }
                    if (dic["PaymentType"] != null)
                    {
                        outgoingTransfers = outgoingTransfers
                            .Where(x => x.PaymentType == (PaymentType)int.Parse(dic["PaymentType"].ToString()));
                    }

                    if (dic["ClientId"] != null)
                    {
                        outgoingTransfers = outgoingTransfers
                            .Where(x => x.FromClientId == int.Parse(dic["ClientId"].ToString()));
                    }

                    if (dic["CompanyId"] != null)
                    {
                        outgoingTransfers = outgoingTransfers
                            .Where(x => x.FromCompanyId == int.Parse(dic["CompanyId"].ToString()));
                    }

                    if (dic["CountryId"] != null)
                    {
                        outgoingTransfers = outgoingTransfers
                            .Where(x => x.CountryId == int.Parse(dic["CountryId"].ToString()));
                    }

                    if (dic["Beneficiary"] != null)
                    {
                        outgoingTransfers = outgoingTransfers
                            .Where(x => x.Beneficiary != null && x.Beneficiary.Name.Contains(dic["Beneficiary"].ToString()));
                    }

                    if (dic["BeneficiaryAddress"] != null)
                    {
                        outgoingTransfers = outgoingTransfers
                            .Where(x => x.Beneficiary != null && x.Beneficiary.Address.Contains(dic["BeneficiaryAddress"].ToString()));
                    }

                    if (dic["Sender"] != null)
                    {
                        outgoingTransfers = outgoingTransfers
                            .Where(x => x.Sender != null && x.Sender.Name.Contains(dic["Sender"].ToString()));
                    }
                }

                return outgoingTransfers.ToList();
            }

            return new List<OutgoingTransfer>();
        }

        IQueryable<OutgoingTransfer> GetAllWithDetails()
        {
            return _outgoingTransferRepository
                .GetAllIncluding(
                tc => tc.ToCompany,
                ttc => ttc.FromCompany,
                fc => fc.FromClient,
                co => co.Country,
                cu => cu.Currency,
                be => be.Beneficiary,
                se => se.Sender
                );
        }

        private OutgoingTransfer GetByIdWithDetail(int id)
        {
            return _outgoingTransferRepository.
                GetAllIncluding(
                    tm => tm.ToCompany,
                    fc => fc.FromClient,
                    fm => fm.FromCompany,
                    s => s.Sender,
                    b => b.Beneficiary,
                    ds => ds.Country,
                    cu => cu.Currency,
                    t => t.Treasury)
                .FirstOrDefault(x => x.Id == id);
        }

        public async Task<OutgoingTransfer> CreateAsync(OutgoingTransfer input)
        {
            var createdOutgoingTransfer = new OutgoingTransfer();
            var toCompany = await _companyManager.GetByIdAsync(input.ToCompanyId);
            var currentCurrency = await _currencyManager.GetByIdAsync(input.CurrencyId);

            //Date and time
            var currentDate = DateTime.Now;
            input.Date = new DateTime(
                input.Date.Year,
                input.Date.Month,
                input.Date.Day,
                currentDate.Hour,
                currentDate.Minute,
                currentDate.Second
                );

            if (toCompany.TenantCompanyId == null)
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    input.Status = OutgoingTransferStatus.Accepted;
                    var id = await _outgoingTransferRepository.InsertAndGetIdAsync(input);

                    createdOutgoingTransfer = await _outgoingTransferRepository.GetAsync(id);

                    IOutgoingTransferDomainService service = _outgoingTransferFactory.CreateService(createdOutgoingTransfer);
                    await service.CreateCashFlowAsync(createdOutgoingTransfer);
                    unitOfWork.Complete();
                }
            }
            else
            {
                IList<Currency> toTenantCurrencies = new List<Currency>();
                using (_unitOfWorkManager.Current.SetTenantId(toCompany.TenantCompanyId))
                {
                    _unitOfWorkManager.Current.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                    toTenantCurrencies = await _currencyManager.GetAllAsync();
                }

                if (!toTenantCurrencies.Any(x => x.Name.ToLower().Trim() == currentCurrency.Name.ToLower().Trim()))
                {
                    throw new UserFriendlyException("الشركة المرسلة إليها هذه الحوالة لا تتعامل بهذه العملة");
                }

                input.Status = OutgoingTransferStatus.Pending;
                var id = await _outgoingTransferRepository.InsertAndGetIdAsync(input);
                createdOutgoingTransfer = await _outgoingTransferRepository.GetAsync(id);

                var data = new CreateExternalTransferEventData(
                    createdOutgoingTransfer.Date,createdOutgoingTransfer.Note,createdOutgoingTransfer.PaymentType,
                    createdOutgoingTransfer.Amount,createdOutgoingTransfer.Commission,createdOutgoingTransfer.CompanyCommission,
                    createdOutgoingTransfer.ClientCommission,createdOutgoingTransfer.CurrencyId,createdOutgoingTransfer.BeneficiaryId,
                    createdOutgoingTransfer.SenderId,createdOutgoingTransfer.FromCompanyId,createdOutgoingTransfer.ToCompanyId,
                    createdOutgoingTransfer.FromClientId,toCompany.TenantCompanyId
                    );

                await EventBus.Default.TriggerAsync(data);
            }



            return createdOutgoingTransfer;

        }

        public async Task<OutgoingTransfer> UpdateAsync(OutgoingTransfer outgoingTransfer)
        {
            var updatedTreasuryAction = new OutgoingTransfer();

            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                updatedTreasuryAction = await _outgoingTransferRepository.UpdateAsync(outgoingTransfer);

                IOutgoingTransferDomainService service = _outgoingTransferFactory.CreateService(updatedTreasuryAction);
                await service.CreateCashFlowAsync(updatedTreasuryAction);

                unitOfWork.Complete();
            }

            return updatedTreasuryAction;
        }

        public async Task<OutgoingTransfer> DeleteAsync(OutgoingTransfer outgoingTransfer)
        {
            var updatedTreasuryAction = new OutgoingTransfer();

            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var cashFlowDeleted = await DeleteCashFlowAsync(outgoingTransfer);
                if (cashFlowDeleted)
                {
                    await _outgoingTransferRepository.DeleteAsync(outgoingTransfer);
                }
                _unitOfWorkManager.Current.SaveChanges();

                unitOfWork.Complete();
            }

            return updatedTreasuryAction;
        }

        public async Task<bool> DeleteCashFlowAsync(OutgoingTransfer outgoingTransfer)
        {
            IOutgoingTransferDomainService service = _outgoingTransferFactory.CreateService(outgoingTransfer);
            return await service.DeleteCashFlowAsync(outgoingTransfer);
        }

        public int GetLastNumber()
        {
            var last = _outgoingTransferRepository.GetAll().OrderByDescending(x => x.Number).FirstOrDefault();
            return last == null ? 0 : last.Number;
        }

        public async Task SetAsCopied(List<int> ids)
        {
            foreach (var id in ids)
            {
                var outgoingTransfer = await _outgoingTransferRepository.GetAsync(id);
                outgoingTransfer.IsCopied = true;

                await _outgoingTransferRepository.UpdateAsync(outgoingTransfer);
            }
        }

        public async Task<List<NotCopiedForCompany>> GetNotCopiedCount()
        {
            IList<Company> companies = new List<Company>();
            var all = GetAllNotCopiedWithDetails();
            companies = await _companyManager.GetAllAsync();
            var companiesNotLinked = companies.Where(x => x.TenantCompanyId == null).ToList();

            List<NotCopiedForCompany> data = new List<NotCopiedForCompany>();

            foreach (var company in companiesNotLinked)
            {
                var allNotCopied = all.Where(x => x.ToCompanyId == company.Id).ToList();
                var allNotCopiedCount = allNotCopied.Count;

                var message = string.IsNullOrEmpty(initialTransferCuurenyCountForCompany(allNotCopied)) ? "0" : initialTransferCuurenyCountForCompany(allNotCopied);

                var companyTranfer = new NotCopiedForCompany() { Name = company.Name + "   " + message, Id = company.Id };
                data.Add(companyTranfer);
            }
            return data;
        }


        #region Helper Methods
        private string initialTransferCuurenyCountForCompany(List<OutgoingTransfer> transfers)
        {
            string message = "";
            Dictionary<string, int> transferCurrencies = new Dictionary<string, int>();

            foreach (var item in transfers)
            {
                if (item.Currency != null)
                {
                    if (transferCurrencies.ContainsKey(item.Currency.Name))
                    {
                        transferCurrencies[item.Currency.Name] = transferCurrencies[item.Currency.Name] + 1;
                    }
                    else
                    {
                        transferCurrencies.Add(item.Currency.Name, 1);
                    }
                }
            }

            foreach (var currencyCount in transferCurrencies)
            {
                if (string.IsNullOrEmpty(message))
                {
                    message = "(" + currencyCount.Value.ToString() + " " + currencyCount.Key + ")";
                }
                else
                {
                    message = message + "  " + "(" + currencyCount.Value.ToString() + " " + currencyCount.Key + ")";
                }
            }

            return message;
        }

        private IQueryable<OutgoingTransfer> GetAllNotCopiedWithDetails()
        {
            return _outgoingTransferRepository
                .GetAllIncluding(cu => cu.Currency).Include(x => x.ToCompany)
                .Where(x => x.IsCopied == false);
        }
        #endregion

    }
}


public class NotCopiedForCompany
{
    public string Name { get; set; }
    public int Id { get; set; }

}