using Abp.Events.Bus;
using System;

namespace Bwr.Exchange.Transfers.OutgoingTransfers.Events
{
    public class CreateExternalTransferEventData : EventData
    {
        public CreateExternalTransferEventData(DateTime date, string note,
            PaymentType paymentType, double amount, double commission,
            double companyCommission, double clientCommission, int currencyId,
            int? beneficiaryId, int? senderId, int? fromCompanyId, int? toCompanyId,
            int? fromClientId, int? toTenantId)
        {
            Date = date;
            Note = note;
            PaymentType = paymentType;
            Amount = amount;
            Commission = commission;
            CompanyCommission = companyCommission;
            ClientCommission = clientCommission;
            CurrencyId = currencyId;
            BeneficiaryId = beneficiaryId;
            SenderId = senderId;
            FromCompanyId = fromCompanyId;
            ToCompanyId = toCompanyId;
            FromClientId = fromClientId;
            ToTenantId = toTenantId;
        }

        public int? ToTenantId { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public PaymentType PaymentType { get; set; }
        public double Amount { get; set; }
        public double Commission { get; set; }
        public double CompanyCommission { get; set; }
        public double ClientCommission { get; set; }
        //public int Number { get; set; }

        public int CurrencyId { get; set; }
        public int? BeneficiaryId { get; set; }
        public int? SenderId { get; set; }
        public int? FromCompanyId { get; set; }
        public int? ToCompanyId { get; set; }
        public int? FromClientId { get; set; }
    }
}
