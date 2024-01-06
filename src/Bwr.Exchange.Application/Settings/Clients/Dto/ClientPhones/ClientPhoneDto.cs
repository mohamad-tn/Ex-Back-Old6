﻿using Abp.Application.Services.Dto;

namespace Bwr.Exchange.Settings.Clients.Dto.ClientPhones
{
    public class ClientPhoneDto : EntityDto
    {
        public int? TenantId { get; set; }
        public int ClientId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
