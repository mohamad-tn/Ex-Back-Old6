using Abp.Application.Services.Dto;
using Bwr.Exchange.Shared.Dto;
using System.Collections.Generic;

namespace Bwr.Exchange.Customers.Dto
{
    public class CustomerWithImagesDto: EntityDto
    {
        public CustomerWithImagesDto()
        {
            Images = new List<FileUploadDto>();
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentificationNumber { get; set; }

        public IList<FileUploadDto> Images { get; set; }
    }
}
