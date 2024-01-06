using Abp.Events.Bus;
using System;

namespace Bwr.Exchange.Transfers.OutgoingTransfers.Events
{
    public class CreateExternalTransferEventData : EventData
    {
        public CreateExternalTransferEventData(DateTime date, string note,
            PaymentType paymentType, double amount, double commission,
            double companyCommission, double clientCommission, string currencyName,
            string beneficiaryName, string senderName,
            int? fromTenantId, int? toTenantId, string fromTenantName, int outgoingTransferId)
        {
            Date = date;
            Note = note;
            PaymentType = paymentType;
            Amount = amount;
            Commission = commission;
            CompanyCommission = companyCommission;
            ClientCommission = clientCommission;
            CurrencyName = currencyName;
            BeneficiaryName = beneficiaryName;
            SenderName = senderName;
            FromTenantId = fromTenantId;
            ToTenantId = toTenantId;
            FromTenantName = fromTenantName;
            OutgoingTransferId = outgoingTransferId;
        }

        public int? ToTenantId { get; set; }
        public int? FromTenantId { get; set; }
        public string FromTenantName { get; set; }
        public int OutgoingTransferId { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public PaymentType PaymentType { get; set; }
        public double Amount { get; set; }
        public double Commission { get; set; }
        public double CompanyCommission { get; set; }
        public double ClientCommission { get; set; }
        public string CurrencyName { get; set; }
        public string SenderName { get; set; }
        public string BeneficiaryName { get; set; }
    }
}
