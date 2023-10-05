﻿using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Bwr.Exchange.CashFlows.CompanyCashFlows.Events;
using Bwr.Exchange.CashFlows.TreasuryCashFlows.Events;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bwr.Exchange.TreasuryActions.Services.Implement
{
    public class SpendMainAccountCompanyExchangePartyTreasuryDomainService : ITreasuryActionDomainService
    {
        private readonly IRepository<TreasuryAction> _treasuryActionRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public TreasuryAction TreasuryAction { get; set; }

        public SpendMainAccountCompanyExchangePartyTreasuryDomainService(
            IRepository<TreasuryAction> treasuryActionRepository,
            IUnitOfWorkManager unitOfWorkManager,
            TreasuryAction treasuryAction
            )
        {
            _treasuryActionRepository = treasuryActionRepository;
            _unitOfWorkManager = unitOfWorkManager;
            TreasuryAction = treasuryAction;
        }

        public async Task<TreasuryAction> CreateTreasuryActionAsync()
        {
            int treasuryActionId;
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                treasuryActionId = await _treasuryActionRepository.InsertAndGetIdAsync(TreasuryAction);

                TreasuryAction = GetByIdWithDetail(treasuryActionId);

                EventBus.Default.Trigger(
                    new TreasuryCashFlowCreatedEventData(
                        TreasuryAction.Date,
                        TreasuryAction.CurrencyId,
                        TreasuryAction.TreasuryId,
                        treasuryActionId,
                        CashFlows.TransactionType.TreasuryAction,
                        (TreasuryAction.Amount * -1),
                        TransactionConst.CompanyReceivable,
                        TreasuryAction.MainAccountCompany.Name
                        ));

                EventBus.Default.Trigger(
                    new CompanyCashFlowCreatedEventData(
                        TreasuryAction.Date,
                        TreasuryAction.CurrencyId,
                        TreasuryAction.MainAccountCompanyId,
                        treasuryActionId,
                        CashFlows.TransactionType.TreasuryAction,
                        TreasuryAction.Amount,
                        TransactionConst.Spend
                        ));

                unitOfWork.Complete();
            }
            return TreasuryAction;
        }

        private TreasuryAction GetByIdWithDetail(int id)
        {
            var teasuryAction = _treasuryActionRepository.Get(id);
            _treasuryActionRepository.EnsurePropertyLoaded(teasuryAction, x => x.MainAccountCompany);

            return teasuryAction;
        }

        public async Task<TreasuryAction> UpdateTreasuryActionAsync()
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var updatedTreasuryAction = await _treasuryActionRepository.UpdateAsync(TreasuryAction);
                _treasuryActionRepository.EnsurePropertyLoaded(updatedTreasuryAction, x => x.MainAccountCompany);
                //var treasuryAction = GetByIdWithDetail(updatedTreasuryAction.Id);

                EventBus.Default.Trigger(
                    new TreasuryCashFlowCreatedEventData(
                        updatedTreasuryAction.Date,
                        updatedTreasuryAction.CurrencyId,
                        updatedTreasuryAction.TreasuryId,
                        updatedTreasuryAction.Id,
                        CashFlows.TransactionType.TreasuryAction,
                        (updatedTreasuryAction.Amount * -1),
                        TransactionConst.CompanyReceivable,
                        updatedTreasuryAction.MainAccountCompany.Name
                        ));

                EventBus.Default.Trigger(
                    new CompanyCashFlowCreatedEventData(
                        TreasuryAction.Date,
                        TreasuryAction.CurrencyId,
                        TreasuryAction.MainAccountCompanyId,
                        TreasuryAction.Id,
                        CashFlows.TransactionType.TreasuryAction,
                        TreasuryAction.Amount,
                        TransactionConst.Spend
                        ));

                unitOfWork.Complete();
            }
            return TreasuryAction;
        }
    }
}
