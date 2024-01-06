using Abp.Application.Services.Dto;
using System;

namespace Bwr.Exchange.Transfers.ExternalTransfers.Dto
{
    public class ExternalTransferDto: EntityDto
    {
        public PaymentType PaymentType { get; set; }
        public double Amount { get; set; }
        public double Commission { get; set; }
        public double CompanyCommission { get; set; }
        public double ClientCommission { get; set; }
        public string Date { get; set; }
        public string Note { get; set; }
        public int? FromTenantId { get; set; }
        public string FromTenantName { get; set; }
        public int OutgoingTransferId { get; set; }
        public string SenderName { get; set; }
        public string BeneficiaryName { get; set; }
        public string CurrencyName { get; set; }
    }
}
