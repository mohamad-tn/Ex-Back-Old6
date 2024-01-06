﻿using Abp.Application.Services;
using Bwr.Exchange.Shared.DataManagerRequests;
using Bwr.Exchange.Shared.Dto;
using Bwr.Exchange.Transfers.OutgoingTransfers.Dto;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bwr.Exchange.Transfers.OutgoingTransfers
{
    public interface IOutgoingTransferAppService : IApplicationService
    {
        Task<OutgoingTransferDto> CreateAsync(OutgoingTransferDto input);
        Task<OutgoingTransferDto> UpdateAsync(OutgoingTransferDto input);
        Task<OutgoingTransferDto> GetForEditAsync(int id);
        Task<IList<OutgoingTransferDto>> Get(SearchOutgoingTransferInputDto input);
        IList<ReadOutgoingTransferDto> GetForStatment(SearchOutgoingTransferInputDto input);
        ReadGrudDto GetForGrid([FromBody] BWireDataManagerRequest dm);
        ReadGrudDto GetForSendingGrid([FromBody] SendingOutgoingDataManagerRequest dm);
        OutgoingTransferDto GetById(int id);
        Task SetAsCopiedAsync(List<int> ids);
        Task<List<NotCopiedForCompany>> GetNotCopiedCount();
        int GetLastNumber();
    }
}
