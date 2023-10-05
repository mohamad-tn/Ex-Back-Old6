using Bwr.Exchange.Settings.Countries.Dto.Provinces;
using Bwr.Exchange.Settings.Countries.Services;
using System.Collections.Generic;
using System.Linq;

namespace Bwr.Exchange.Settings.Countries
{
    public class ProvinceAppService : ExchangeAppServiceBase, IProvinceAppService
    {
        private readonly ICountryManager _countryManager;

        public ProvinceAppService(ICountryManager countryManager)
        {
            _countryManager = countryManager;
        }

        public IList<ProvinceForDropdownDto> GetAllForDropdown()
        {
            var provinces = _countryManager.GetAllWithDetail().ToList().SelectMany(x => x.Provinces);
            return (from province in provinces
                    select new ProvinceForDropdownDto()
                    {
                        CountryName = province.Country != null ? province.Country.Name : string.Empty,
                        Name = province.Name,
                        Id = province.Id
                    }).ToList();
        }
    }
}
