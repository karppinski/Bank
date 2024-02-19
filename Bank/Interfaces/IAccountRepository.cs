using Bank.Models;

namespace Bank.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAccounts();
    }
}
