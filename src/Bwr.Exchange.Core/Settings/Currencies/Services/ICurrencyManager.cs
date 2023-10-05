using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bwr.Exchange.Settings.Currencies.Services
{
    public interface ICurrencyManager : IDomainService
    {
        Task<IList<Currency>> GetAllAsync();
        Task<Currency> GetByIdAsync(int id);
        IList<Currency> GetAll();
        Task<Currency> InsertAndGetAsync(Currency country);
        Task<Currency> UpdateAndGetAsync(Currency country);
        Task DeleteAsync(int id);
        bool CheckIfNameAlreadyExist(string name);
        bool CheckIfNameAlreadyExist(int id, string name);
    }
}
