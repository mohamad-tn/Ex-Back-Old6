﻿using Bwr.Exchange.Customers.Dto;
using Bwr.Exchange.Customers.Services;
using Bwr.Exchange.Shared.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bwr.Exchange.Customers
{
    public class CustomerAppService : ExchangeAppServiceBase, ICustomerAppService
    {
        private readonly ICustomerManager _customerManager;
        private readonly ICustomerImageManager _customerImageManager;

        public CustomerAppService(
            ICustomerManager customerManager,
            ICustomerImageManager customerImageManager)
        {
            _customerManager = customerManager;
            _customerImageManager = customerImageManager;
        }

        public async Task<IList<CustomerDto>> GetAllAsync()
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                var customers = await _customerManager.GetAllAsync();
                return ObjectMapper.Map<List<CustomerDto>>(customers);
            }
        }

        public async Task<CustomerDto> GetByNameAsync(string symbol)
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                var customer = await _customerManager.GetByNameAsync(symbol);
                return ObjectMapper.Map<CustomerDto>(customer);
            }
        }

        public async Task<IList<FileUploadDto>> GetCustomerImagesAsync(int customerId)
        {
            IList<CustomerImage> customerImages = new List<CustomerImage>();
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                customerImages = await _customerImageManager.GetAllAsync(customerId);
            }
            var images = (from ci in customerImages
                          select new FileUploadDto()
                          {
                              FileName = ci.Name,
                              FilePath = ci.Path,
                              FileType = ci.Type,
                              FileSize = ci.Size
                          });

            return images.ToList();
        }

        public async Task<CustomerWithImagesDto> GetCustomerWithImagesAsync(int id)
        {
            Customer customer = new Customer();
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                CurrentUnitOfWork.DisableFilter(Abp.Domain.Uow.AbpDataFilters.MayHaveTenant);
                customer = await _customerManager.GetCustomerWithImages(id);
            }
            var images = await GetCustomerImagesAsync(customer.Id);
            var customerDto = ObjectMapper.Map<CustomerWithImagesDto>(customer);
            customerDto.Images = images;

            return customerDto;
        }

        public IList<CustomerDto> GetTreasuryActionBeneficiaries()
        {
            return new List<CustomerDto>();
        }
    }
}
