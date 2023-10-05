using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Bwr.Exchange.Transfers.OutgoingTransfers.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bwr.Exchange.Transfers.OutgoingTransfers.Services
{
    public class OutgoingTransferManager : IOutgoingTransferManager
    {
        private readonly IRepository<OutgoingTransfer> _outgoingTransferRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IOutgoingTransferFactory _outgoingTransferFactory;
        public IEventBus EventBus { get; set; }

        public OutgoingTransferManager(
            IRepository<OutgoingTransfer> outgoingTransferRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IOutgoingTransferFactory outgoingTransferFactory
            )
        {
            _outgoingTransferRepository = outgoingTransferRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _outgoingTransferFactory = outgoingTransferFactory;
            EventBus = NullEventBus.Instance;
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
                ).Where(x=>x.Id == id).FirstOrDefault();
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
                    if (dic["FromDate"] != null && dic["FromDate"] != null)
                    {
                        outgoingTransfers = outgoingTransfers.Where(x =>
                          x.Date >= DateTime.Parse(dic["FromDate"].ToString()) &&
                          x.Date <= DateTime.Parse(dic["ToDate"].ToString())).ToList();
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
                    cu=>cu.Currency,
                    t=>t.Treasury)
                .FirstOrDefault(x => x.Id == id);
        }

        public async Task<OutgoingTransfer> CreateAsync(OutgoingTransfer input)
        {
            var createdOutgoingTransfer = new OutgoingTransfer();
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
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

                var id = await _outgoingTransferRepository.InsertAndGetIdAsync(input);

                createdOutgoingTransfer = await _outgoingTransferRepository.GetAsync(id);

                IOutgoingTransferDomainService service = _outgoingTransferFactory.CreateService(createdOutgoingTransfer);
                await service.CreateCashFlowAsync(createdOutgoingTransfer);

                unitOfWork.Complete();
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
    }
}
