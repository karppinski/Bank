using Bank.Dtos.Account;
using Bank.Models;

namespace Bank.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAccounts();
        Task<List<Account>> GetAccountsForAnUser(string id);
        Task<Account> GetUserAccountWithId(string userId, int accountId);
        Task<Account> GetAccountWithId(int id);
        Task DeleteAccountsForUserId(string userId);
        Task DeleteAccountById(int id);
        Task<Account> CreateAccount(string userId);
    }
}
