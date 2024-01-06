using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Handlers;
using Abp.UI;
using Bwr.Exchange.Transfers.ExternalTransfers;
using Bwr.Exchange.Transfers.ExternalTransfers.Services;
using System.Threading.Tasks;

namespace Bwr.Exchange.Transfers.OutgoingTransfers.Events
{
    public class CreateExternalTransferEventHandler : IAsyncEventHandler<CreateExternalTransferEventData>, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IExternalTreansferManager _externalTreansferManager;

        public CreateExternalTransferEventHandler(IUnitOfWorkManager unitOfWorkManager, IExternalTreansferManager externalTreansferManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _externalTreansferManager = externalTreansferManager;
        }

        public async Task HandleEventAsync(CreateExternalTransferEventData eventData)
        {
            ExtrenalTransfer extrenalTransfer = new ExtrenalTransfer();
            extrenalTransfer.Note = eventData.Note;
            extrenalTransfer.Date = eventData.Date;
            extrenalTransfer.FromCompanyId = eventData.FromCompanyId;
            extrenalTransfer.ToCompanyId = eventData.ToCompanyId;
            extrenalTransfer.FromClientId = eventData.FromClientId;
            extrenalTransfer.CurrencyId = eventData.CurrencyId;
            extrenalTransfer.BeneficiaryId = eventData.BeneficiaryId;
            extrenalTransfer.SenderId = eventData.SenderId;
            extrenalTransfer.PaymentType = eventData.PaymentType;
            extrenalTransfer.Commission = eventData.Commission;
            extrenalTransfer.CompanyCommission = eventData.CompanyCommission;
            extrenalTransfer.ClientCommission = eventData.ClientCommission;
            extrenalTransfer.Amount = eventData.Amount;

            using (_unitOfWorkManager.Current.SetTenantId(eventData.ToTenantId))
            {
                _unitOfWorkManager.Current.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                var createdExternalTransfer = await _externalTreansferManager.InsertAndGetAsync(extrenalTransfer);
                if (createdExternalTransfer == null)
                    throw new UserFriendlyException("Error");
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }
    }
}
