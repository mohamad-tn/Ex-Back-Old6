using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Handlers;
using Abp.UI;
using Bwr.Exchange.Settings.Companies.Services;
using Bwr.Exchange.Transfers.ExternalTransfers;
using System.Threading.Tasks;

namespace Bwr.Exchange.Transfers.OutgoingTransfers.Events
{
    public class CreateExternalTransferEventHandler : IAsyncEventHandler<CreateExternalTransferEventData>, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ICompanyManager _companyManager;
        private readonly IRepository<ExtrenalTransfer> _externalTreansferRepository;

        public CreateExternalTransferEventHandler(IUnitOfWorkManager unitOfWorkManager, IRepository<ExtrenalTransfer> externalTreansferRepository, ICompanyManager companyManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _externalTreansferRepository = externalTreansferRepository;
            _companyManager = companyManager;
        }

        public async Task HandleEventAsync(CreateExternalTransferEventData eventData)
        {
            ExtrenalTransfer extrenalTransfer = new ExtrenalTransfer();
            extrenalTransfer.Note = eventData.Note;
            extrenalTransfer.Date = eventData.Date;
            extrenalTransfer.FromTenantId = eventData.FromTenantId;
            extrenalTransfer.CurrencyName = eventData.CurrencyName;
            extrenalTransfer.BeneficiaryName = eventData.BeneficiaryName;
            extrenalTransfer.SenderName = eventData.SenderName;
            extrenalTransfer.PaymentType = eventData.PaymentType;
            extrenalTransfer.Commission = eventData.Commission;
            extrenalTransfer.CompanyCommission = eventData.CompanyCommission;
            extrenalTransfer.ClientCommission = eventData.ClientCommission;
            extrenalTransfer.Amount = eventData.Amount;
            extrenalTransfer.OutgoingTransferId = eventData.OutgoingTransferId;

            using (_unitOfWorkManager.Current.SetTenantId(eventData.ToTenantId))
            {
                _unitOfWorkManager.Current.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);

                var companyRelatedWithFromTenantId = _companyManager.GetByCompanyTenantId((int)eventData.FromTenantId);
                extrenalTransfer.FromTenantName = companyRelatedWithFromTenantId.Name;

                var createdExternalTransfer = await _externalTreansferRepository.InsertAsync(extrenalTransfer);
                if (createdExternalTransfer == null)
                    throw new UserFriendlyException("Error");
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }
    }
}
