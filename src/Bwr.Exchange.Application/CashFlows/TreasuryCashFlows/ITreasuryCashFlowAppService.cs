using Abp.Application.Services;
using Bwr.Exchange.CashFlows.Shared.Dto;
using Bwr.Exchange.CashFlows.TreasuryCashFlows.Dto;
using Bwr.Exchange.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bwr.Exchange.CashFlows.TreasuryCashFlows
{
    public interface ITreasuryCashFlowAppService : IApplicationService
    {
        IList<TreasuryCashFlowDto> Get(GetTreasuryCashFlowInput input);
        Task<IList<SummaryCashFlowDto>> Summary(string date);
        ReadGrudDto GetForGrid([FromBody] DataManagerRequest dm);
        //Task<TreasuryCashFlowMatchingDto> MatchAsync(TreasuryCashFlowMatchingDto input);
    }
}
