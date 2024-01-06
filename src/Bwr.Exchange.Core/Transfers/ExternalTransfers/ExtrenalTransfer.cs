using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Bwr.Exchange.Settings.Clients;
using Bwr.Exchange.Settings.Companies;
using Bwr.Exchange.Settings.Currencies;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bwr.Exchange.Transfers.ExternalTransfers
{
    public class ExtrenalTransfer : AuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public PaymentType PaymentType { get; set; }
        public double Amount { get; set; }
        public double Commission { get; set; }
        public double CompanyCommission { get; set; }
        public double ClientCommission { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public int? FromTenantId { get; set; }
        public string FromTenantName { get; set; }
        public int OutgoingTransferId { get; set; }
        public string SenderName { get; set; }
        public string BeneficiaryName { get; set; }
        public string CurrencyName { get; set; }
    }
}
