using Abp.Application.Services.Dto;
using Bwr.Exchange.Customers.Dto;
using Bwr.Exchange.Settings.Clients.Dto;
using Bwr.Exchange.Settings.Companies.Dto;
using Bwr.Exchange.Settings.Countries.Dto;
using Bwr.Exchange.Settings.Currencies.Dto;
using Bwr.Exchange.Settings.Treasury.Dto;

namespace Bwr.Exchange.Transfers.OutgoingTransfers.Dto
{
    public class ReadOutgoingTransferDto : EntityDto
    {
        public int Number { get; set; }
        public int PaymentType { get; set; }
        public double Amount { get; set; }
        public double Commission { get; set; }
        public double CompanyCommission { get; set; }
        public double ClientCommission { get; set; }
        public string Date { get; set; }
        public double ReceivedAmount { get; set; }
        public string InstrumentNo { get; set; } //رقم الصك
        public string Reason { get; set; }
        public string Note { get; set; }
        public bool IsCopied { get; set; }

        public int CountryId { get; set; }
        public CountryDto Country { get; set; }

        public int CurrencyId { get; set; }
        public CurrencyDto Currency { get; set; }

        public int? TreasuryId { get; set; }
        public TreasuryDto Treasury { get; set; }


        public int? BeneficiaryId { get; set; }
        public CustomerDto Beneficiary { get; set; }

        public int? SenderId { get; set; }
        public CustomerDto Sender { get; set; }
        public int? FromCompanyId { get; set; }
        public CompanyDto FromCompany { get; set; }

        public int? ToCompanyId { get; set; }
        public CompanyDto ToCompany { get; set; }

        public int? FromClientId { get; set; }
        public ClientDto FromClient { get; set; }
    }
}
