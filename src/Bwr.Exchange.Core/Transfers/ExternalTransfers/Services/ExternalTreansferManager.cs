using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Bwr.Exchange.Transfers.OutgoingTransfers.Services;
using Bwr.Exchange.Transfers.OutgoingTransfers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bwr.Exchange.Transfers.OutgoingTransfers.Factories;

namespace Bwr.Exchange.Transfers.ExternalTransfers.Services
{
    public class ExternalTreansferManager : IExternalTreansferManager
    {
        private readonly IRepository<ExtrenalTransfer> _externalTrransferRepository;
        private readonly IOutgoingTransferManager _outgoingTransferManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IOutgoingTransferFactory _outgoingTransferFactory;

        public ExternalTreansferManager(IRepository<ExtrenalTransfer> externalTrransferRepository, IOutgoingTransferManager outgoingTransferManager, IUnitOfWorkManager unitOfWorkManager, IOutgoingTransferFactory outgoingTransferFactory)
        {
            _externalTrransferRepository = externalTrransferRepository;
            _outgoingTransferManager = outgoingTransferManager;
            _unitOfWorkManager = unitOfWorkManager;
            _outgoingTransferFactory = outgoingTransferFactory;
        }

        public IList<ExtrenalTransfer> GetAll()
        {
            return _externalTrransferRepository.GetAll().ToList();
        }

        public async Task<IList<ExtrenalTransfer>> GetAllAsync()
        {
            return await _externalTrransferRepository.GetAllListAsync();
        }

        public async Task<ExtrenalTransfer> GetByIdAsync(int id)
        {
            return await _externalTrransferRepository.GetAsync(id);
        }

        public async Task<ExtrenalTransfer> InsertAndGetAsync(ExtrenalTransfer extrenalTransfer)
        {
            return await _externalTrransferRepository.InsertAsync(extrenalTransfer);
        }

        public async Task DeleteAsync(int id)
        {
            var externalTransfer = await GetByIdAsync(id);
            if(externalTransfer != null)
            await _externalTrransferRepository.DeleteAsync(externalTransfer);
        }

        public async Task AcceptExternalTransferAsync(int id)
        {
            var externalTransfer = await _externalTrransferRepository.GetAsync(id);

            using (_unitOfWorkManager.Current.SetTenantId(externalTransfer.FromTenantId))
            {
                _unitOfWorkManager.Current.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                var outgoingTransfer = _outgoingTransferManager.GetById(externalTransfer.OutgoingTransferId);


                    outgoingTransfer.Status = OutgoingTransferStatus.Accepted;
                    await _outgoingTransferManager.UpdateAsync(outgoingTransfer);

                    IOutgoingTransferDomainService service = _outgoingTransferFactory.CreateService(outgoingTransfer);
                    await service.CreateCashFlowAsync(outgoingTransfer);

                _unitOfWorkManager.Current.SaveChanges();
            }

            //var destinationCurrency = await _currencyRepository
            //    .FirstOrDefaultAsync(x => x.Name.Trim().ToLower() == outgoingTransfer.Currency.Name.ToLower().Trim());

            //var data = new CreateIncomeTransferWhenAcceptOutgoingEventData
            //    (
            //        outgoingTransfer.ToCompanyId,
            //        outgoingTransfer.Date.ToString(),
            //        outgoingTransfer.Note,
            //        null, null, 0, 0, 0,
            //        outgoingTransfer.Amount,
            //        outgoingTransfer.Commission,
            //        outgoingTransfer.CompanyCommission, destinationCurrency.Id,
            //        outgoingTransfer.BeneficiaryId,
            //        outgoingTransfer.SenderId, 0, branchId,
            //        outgoingTransfer.Sender,
            //        outgoingTransfer.Beneficiary

            //    );

            //await EventBus.Default.TriggerAsync(data);
        }
    }
}
