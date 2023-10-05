﻿using Abp.UI;
using Bwr.Exchange.CashFlows.ClientCashFlows.Services;
using Bwr.Exchange.Settings.Clients.Dto;
using Bwr.Exchange.Settings.Clients.Dto.ClientBalances;
using Bwr.Exchange.Settings.Clients.Services;
using Bwr.Exchange.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bwr.Exchange.Settings.Clients
{
    public class ClientAppService : ExchangeAppServiceBase, IClientAppService
    {
        private readonly IClientManager _clientManager;
        private readonly IClientCashFlowManager _clientCashFlowManager;
        public ClientAppService(
            IClientManager clientManager, 
            IClientCashFlowManager clientCashFlowManager
            )
        {
            _clientManager = clientManager;
            _clientCashFlowManager = clientCashFlowManager;
        }

        public async Task<IList<ClientDto>> GetAllAsync()
        {
            var countries = await _clientManager.GetAllAsync();

            return ObjectMapper.Map<List<ClientDto>>(countries);
        }

        public async Task<ClientBalanceDto> GetBalanceForEdit(ClientBalanceForEditInputDto input)
        {
            var outgoingTransfer = await _clientCashFlowManager.GetByTransctionInfo(input.Id, input.TransactionType);
            return new ClientBalanceDto()
            {
                Balance = outgoingTransfer.CurrentBalance,
                ClientId = outgoingTransfer.ClientId,
                CurrencyId = outgoingTransfer.CurrencyId
            };
        }

        [HttpPost]
        public ReadGrudDto GetForGrid([FromBody] DataManagerRequest dm)
        {
            var data = _clientManager.GetAllWithDetail();
            IEnumerable<ReadClientDto> countries = ObjectMapper.Map<List<ReadClientDto>>(data);

            var operations = new DataOperations();
            if (dm.Where != null && dm.Where.Count > 0)
            {
                countries = operations.PerformFiltering(countries, dm.Where, "and");
            }

            if (dm.Sorted != null && dm.Sorted.Count > 0)
            {
                countries = operations.PerformSorting(countries, dm.Sorted);
            }

            IEnumerable groupDs = new List<ReadClientDto>();
            if (dm.Group != null)
            {
                groupDs = operations.PerformSelect(countries, dm.Group);
            }

            var count = countries.Count();

            if (dm.Skip != 0)
            {
                countries = operations.PerformSkip(countries, dm.Skip);
            }

            if (dm.Take != 0)
            {
                countries = operations.PerformTake(countries, dm.Take);
            }

            return new ReadGrudDto() { result = countries, count = count, groupDs = groupDs };
        }
        public UpdateClientDto GetForEdit(int id)
        {
            var client =  _clientManager.GetByIdWithDetail(id);
            return ObjectMapper.Map<UpdateClientDto>(client);
        }
        public async Task<ClientDto> CreateAsync(CreateClientDto input)
        {
            CheckBeforeCreate(input);

            var client = ObjectMapper.Map<Client>(input);

            var createdClient = await _clientManager.InsertAndGetAsync(client);

            return ObjectMapper.Map<ClientDto>(createdClient);
        }
        public async Task<ClientDto> UpdateAsync(UpdateClientDto input)
        {
            CheckBeforeUpdate(input);

            var client = await _clientManager.GetByIdAsync(input.Id);

            ObjectMapper.Map<UpdateClientDto, Client>(input, client);

            var updatedClient = await _clientManager.UpdateAndGetAsync(client);

            return ObjectMapper.Map<ClientDto>(updatedClient);
        }
        public async Task DeleteAsync(int id)
        {
            await _clientManager.DeleteAsync(id);
        }

        #region Helper methods
        private void CheckBeforeCreate(CreateClientDto input)
        {
            var validationResultMessage = string.Empty;

            if (_clientManager.CheckIfNameAlreadyExist(input.Name))
            {
                validationResultMessage = L(ValidationResultMessage.NameAleadyExist);
            }

            if (!string.IsNullOrEmpty(validationResultMessage))
                throw new UserFriendlyException(validationResultMessage);
        }
        private void CheckBeforeUpdate(UpdateClientDto input)
        {
            var validationResultMessage = string.Empty;

            if (_clientManager.CheckIfNameAlreadyExist(input.Id, input.Name))
            {
                validationResultMessage = L(ValidationResultMessage.NameAleadyExist);
            }

            if (!string.IsNullOrEmpty(validationResultMessage))
                throw new UserFriendlyException(validationResultMessage);
        }

        public ClientBalanceDto GetCurrentBalance(CurrentBalanceInputDto input)
        {
            ClientBalanceDto clientBalanceDto = new ClientBalanceDto()
            {
                ClientId = input.ClientId,
                CurrencyId = input.CurrencyId
            };
            clientBalanceDto.Balance = _clientCashFlowManager.GetLastBalance(input.ClientId, input.CurrencyId, DateTime.Now);

            return clientBalanceDto;
        }

        



        #endregion
    }
}
