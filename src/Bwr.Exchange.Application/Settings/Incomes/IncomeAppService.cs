using Abp.Domain.Uow;
using Abp.Threading;
using Abp.UI;
using Bwr.Exchange.CashFlows.ManagementStatement;
using Bwr.Exchange.Settings.Incomes.Dto;
using Bwr.Exchange.Settings.Incomes.Services;
using Bwr.Exchange.Shared.DataManagerRequests;
using Bwr.Exchange.Shared.Dto;
using Bwr.Exchange.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bwr.Exchange.Settings.Incomes
{
    public class IncomeAppService : ExchangeAppServiceBase, IIncomeAppService
    {
        private readonly IIncomeManager _incomeManager;
        public IncomeAppService(IncomeManager incomeManager)
        {
            _incomeManager = incomeManager;
        }

        public async Task<IList<IncomeDto>> GetAllAsync()
        {
            IList<Income> incomes = new List<Income>();
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                incomes = await _incomeManager.GetAllAsync();
            }
            return ObjectMapper.Map<List<IncomeDto>>(incomes);
        }
        [HttpPost]
        public ReadGrudDto GetForGrid([FromBody] BWireDataManagerRequest dm)
        {
            IList<Income> data = new List<Income>();

            using (CurrentUnitOfWork.SetTenantId(dm.tenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                data = _incomeManager.GetAll();
            }
            IEnumerable<ReadIncomeDto> incomes = ObjectMapper.Map<List<ReadIncomeDto>>(data);

            var operations = new DataOperations();
            if (dm.Where != null && dm.Where.Count > 0)
            {
                incomes = operations.PerformFiltering(incomes, dm.Where, "and");
            }

            if (dm.Sorted != null && dm.Sorted.Count > 0)
            {
                incomes = operations.PerformSorting(incomes, dm.Sorted);
            }

            IEnumerable groupDs = new List<ReadIncomeDto>();
            if (dm.Group != null)
            {
                groupDs = operations.PerformSelect(incomes, dm.Group);
            }

            var count = incomes.Count();

            if (dm.Skip != 0)
            {
                incomes = operations.PerformSkip(incomes, dm.Skip);
            }

            if (dm.Take != 0)
            {
                incomes = operations.PerformTake(incomes, dm.Take);
            }

            return new ReadGrudDto() { result = incomes, count = count, groupDs = groupDs };
        }
        public UpdateIncomeDto GetForEdit(int id)
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
               var income = AsyncHelper.RunSync(() => _incomeManager.GetByIdAsync(id));
                return ObjectMapper.Map<UpdateIncomeDto>(income);
            }
        }
        public async Task<IncomeDto> CreateAsync(CreateIncomeDto input)
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                CheckBeforeCreate(input);
            }
            var income = ObjectMapper.Map<Income>(input);
            var createdIncome = await _incomeManager.InsertAndGetAsync(income);            
            return ObjectMapper.Map<IncomeDto>(createdIncome);
        }
        public async Task<IncomeDto> UpdateAsync(UpdateIncomeDto input)
        {
            Income income = new Income("");
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                CheckBeforeUpdate(input);
                income = await _incomeManager.GetByIdAsync(input.Id);
            }

            ObjectMapper.Map<UpdateIncomeDto, Income>(input, income);

            var updatedIncome = await _incomeManager.UpdateAndGetAsync(income);

            return ObjectMapper.Map<IncomeDto>(updatedIncome);
        }
        public async Task DeleteAsync(int id)
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                await _incomeManager.DeleteAsync(id);
            }
        }

        #region Helper methods
        private void CheckBeforeCreate(CreateIncomeDto input)
        {
            var validationResultMessage = string.Empty;

            if (_incomeManager.CheckIfNameAlreadyExist(input.Name))
            {
                validationResultMessage = L(ValidationResultMessage.NameAleadyExist);
            }

            if (!string.IsNullOrEmpty(validationResultMessage))
                throw new UserFriendlyException(validationResultMessage);
        }
        private void CheckBeforeUpdate(UpdateIncomeDto input)
        {
            var validationResultMessage = string.Empty;

            if (_incomeManager.CheckIfNameAlreadyExist(input.Id, input.Name))
            {
                validationResultMessage = L(ValidationResultMessage.NameAleadyExist);
            }

            if (!string.IsNullOrEmpty(validationResultMessage))
                throw new UserFriendlyException(validationResultMessage);
        }

        #endregion
    }
}
