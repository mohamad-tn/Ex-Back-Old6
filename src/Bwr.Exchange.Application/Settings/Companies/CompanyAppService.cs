using Abp.UI;
using Bwr.Exchange.CashFlows.CompanyCashFlows.Services;
using Bwr.Exchange.LinkTenantsCompanies;
using Bwr.Exchange.LinkTenantsCompanies.Dto;
using Bwr.Exchange.LinkTenantsCompanies.Sevices;
using Bwr.Exchange.Settings.Companies.Dto;
using Bwr.Exchange.Settings.Companies.Dto.CompanyBalance;
using Bwr.Exchange.Settings.Companies.Dto.CompanyBalances;
using Bwr.Exchange.Settings.Companies.Services;
using Bwr.Exchange.Settings.Currencies.Services;
using Bwr.Exchange.Settings.Incomes;
using Bwr.Exchange.Shared.DataManagerRequests;
using Bwr.Exchange.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bwr.Exchange.Settings.Companies
{
    public class CompanyAppService : ExchangeAppServiceBase, ICompanyAppService
    {
        private readonly CompanyManager _companyManager;
        private readonly CurrencyManager _currencyManager;
        private readonly CompanyCashFlowManager _companyCashFlowManager;
        private readonly LinkTenantCompanyManager _linkTenantCompanyManager;
        public CompanyAppService(
            CompanyManager companyManager,
            CurrencyManager currencyManager,
            CompanyCashFlowManager companyCashFlowManager,
            LinkTenantCompanyManager linkTenantCompanyManager)
        {
            _companyManager = companyManager;
            _currencyManager = currencyManager;
            _companyCashFlowManager = companyCashFlowManager;
            _linkTenantCompanyManager = linkTenantCompanyManager;
        }

        public async Task<IList<CompanyDto>> GetAllAsync()
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                var companies = await _companyManager.GetAllAsync();
                return ObjectMapper.Map<List<CompanyDto>>(companies);
            }
        }
        public async Task<CompanyBalanceDto> GetBalanceForEdit(CompanyBalanceForEditInputDto input)
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                var outgoingTransfer = await _companyCashFlowManager.GetByTransctionInfo(input.Id, input.TransactionType);
                return new CompanyBalanceDto()
                {
                    Balance = outgoingTransfer.CurrentBalance,
                    CompanyId = outgoingTransfer.CompanyId,
                    CurrencyId = outgoingTransfer.CurrencyId
                };
            }
        }
        [HttpPost]
        public ReadGrudDto GetForGrid([FromBody] BWireDataManagerRequest dm)
        {
            IList<Company> data = new List<Company>();
            using (CurrentUnitOfWork.SetTenantId(dm.tenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                data = _companyManager.GetAll();
            }
            IEnumerable<ReadCompanyDto> companies = ObjectMapper.Map<List<ReadCompanyDto>>(data);

            var operations = new DataOperations();
            if (dm.Where != null && dm.Where.Count > 0)
            {
                companies = operations.PerformFiltering(companies, dm.Where, "and");
            }

            if (dm.Sorted != null && dm.Sorted.Count > 0)
            {
                companies = operations.PerformSorting(companies, dm.Sorted);
            }

            IEnumerable groupDs = new List<ReadCompanyDto>();
            if (dm.Group != null)
            {
                groupDs = operations.PerformSelect(companies, dm.Group);
            }

            var count = companies.Count();

            if (dm.Skip != 0)
            {
                companies = operations.PerformSkip(companies, dm.Skip);
            }

            if (dm.Take != 0)
            {
                companies = operations.PerformTake(companies, dm.Take);
            }

            return new ReadGrudDto() { result = companies, count = count, groupDs = groupDs };
        }
        public CompanyBalanceDto GetCurrentBalance(CompanyBalanceInputDto input)
        {
            CompanyBalanceDto companyBalanceDto = new CompanyBalanceDto()
            {
                CompanyId = input.CompanyId,
                CurrencyId = input.CurrencyId
            };

            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                companyBalanceDto.Balance = _companyCashFlowManager.GetLastBalance(input.CompanyId, input.CurrencyId, DateTime.Now);
            }
            return companyBalanceDto;
        }
        public async Task<IList<CompanyBalanceWithCurrencyDto>> GetAllBalances(int companyId)
        {
            var balances = new List<CompanyBalanceWithCurrencyDto>();

            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                var currencies = await _currencyManager.GetAllAsync();
                foreach (var currency in currencies)
                {
                    var input = new CompanyBalanceInputDto()
                    {
                        CompanyId = companyId,
                        CurrencyId = currency.Id
                    };

                    var balanceDto = GetCurrentBalance(input);
                    if (balanceDto != null)
                    {
                        balances.Add(new CompanyBalanceWithCurrencyDto(balanceDto.Balance, companyId, currency.Id, currency.Name));
                    }
                }
            }
            return balances;
        }
        public UpdateCompanyDto GetForEdit(int id)
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                var company = _companyManager.GetByIdWithDetail(id);
                return ObjectMapper.Map<UpdateCompanyDto>(company);
            }
        }
        public async Task<CompanyDto> CreateAsync(CreateCompanyDto input)
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                CheckBeforeCreate(input);
            }
            var company = ObjectMapper.Map<Company>(input);

            var createdCompany = await _companyManager.InsertAndGetAsync(company);

            return ObjectMapper.Map<CompanyDto>(createdCompany);
        }
        public async Task<CompanyDto> UpdateAsync(UpdateCompanyDto input)
        {
            Company company = new Company();
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                CheckBeforeUpdate(input);

                company = await _companyManager.GetByIdAsync(input.Id);
            }

            ObjectMapper.Map<UpdateCompanyDto, Company>(input, company);

            var updatedCompany = await _companyManager.UpdateAndGetAsync(company);

            return ObjectMapper.Map<CompanyDto>(updatedCompany);
        }
        public async Task DeleteAsync(int id)
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                await _companyManager.DeleteAsync(id);
            }
        }
        public List<CompanyDto> GetAllCompaniesOfTenant(int? tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                var data = _companyManager.GetAll();
                return ObjectMapper.Map<List<CompanyDto>>(data);
            }
        }
        public async Task LinkCompanyWithTenant(LinkTenantCompanyDto input)
        {
            LinkTenantCompany createdOrUpdatedLinkTenantCompany;
            LinkTenantCompany newLinkTenantCompany =
                new LinkTenantCompany(input.FirstTenantId, input.SecondTenantId, input.CompanyId);

            var linkTenantCompany = _linkTenantCompanyManager.GetAll()
                .FirstOrDefault(x => x.FirstTenantId == input.FirstTenantId && x.SecondTenantId == input.SecondTenantId);

            if (linkTenantCompany == null)
            {
                createdOrUpdatedLinkTenantCompany = await _linkTenantCompanyManager.InsertAsync(newLinkTenantCompany);
            }
            else
            {
                using (CurrentUnitOfWork.SetTenantId(input.SecondTenantId))
                {
                    CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                    var company = await _companyManager.GetByIdAsync(linkTenantCompany.CompanyId);
                    company.TenantCompanyId = null;
                    await _companyManager.UpdateAndGetAsync(company);
                    CurrentUnitOfWork.SaveChanges();
                }

                linkTenantCompany.FirstTenantId = input.FirstTenantId;
                linkTenantCompany.SecondTenantId = input.SecondTenantId;
                linkTenantCompany.CompanyId = input.CompanyId;
                createdOrUpdatedLinkTenantCompany = await _linkTenantCompanyManager.UpdateAndGetAsync(linkTenantCompany);
            }

            using (CurrentUnitOfWork.SetTenantId(input.SecondTenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                var company = await _companyManager.GetByIdAsync(input.CompanyId);
                company.TenantCompanyId = input.FirstTenantId;
                company.TenantId = input.SecondTenantId;
                await _companyManager.UpdateAndGetAsync(company);
                CurrentUnitOfWork.SaveChanges();
            }
        }
        public async Task<CompanyDto> GetCompanyLikedWithTenant(int tenantId)
        {
            return new CompanyDto();
        }

        #region Helper methods
        private void CheckBeforeCreate(CreateCompanyDto input)
        {
            var validationResultMessage = string.Empty;

            if (_companyManager.CheckIfNameAlreadyExist(input.Name))
            {
                validationResultMessage = L(ValidationResultMessage.NameAleadyExist);
            }

            if (!string.IsNullOrEmpty(validationResultMessage))
                throw new UserFriendlyException(validationResultMessage);
        }
        private void CheckBeforeUpdate(UpdateCompanyDto input)
        {
            var validationResultMessage = string.Empty;

            if (_companyManager.CheckIfNameAlreadyExist(input.Id, input.Name))
            {
                validationResultMessage = L(ValidationResultMessage.NameAleadyExist);
            }

            if (!string.IsNullOrEmpty(validationResultMessage))
                throw new UserFriendlyException(validationResultMessage);
        }

        #endregion
    }
}
