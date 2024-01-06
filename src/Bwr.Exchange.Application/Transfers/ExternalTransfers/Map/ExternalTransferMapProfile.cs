using AutoMapper;
using Bwr.Exchange.Transfers.ExternalTransfers.Dto;

namespace Bwr.Exchange.Transfers.ExternalTransfers.Map
{
    public class ExternalTransferMapProfile : Profile
    {
        public ExternalTransferMapProfile()
        {
            CreateMap<ExtrenalTransfer, ExternalTransferDto>();
        }
    }
}
