using Bank.Dtos.Account;
using Bank.Models;

namespace Bank.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAccounts();
        Task<List<Account>> GetAccountsForAnUser(string id);
        Task<Account> GetUserAccountWithId(string userId, int accountId);
      //  Task<Account> CreateAccount()
    }
}
