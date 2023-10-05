using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bwr.Exchange.Settings.Expenses.Services
{
    public interface IExpenseManager : IDomainService
    {
        Task<IList<Expense>> GetAllAsync();
        Task<Expense> GetByIdAsync(int id);
        IList<Expense> GetAll();
        Task<Expense> InsertAndGetAsync(Expense expense);
        Task<Expense> UpdateAndGetAsync(Expense expense);
        Task DeleteAsync(int id);
        bool CheckIfNameAlreadyExist(string name);
        bool CheckIfNameAlreadyExist(int id, string name);
    }
}
