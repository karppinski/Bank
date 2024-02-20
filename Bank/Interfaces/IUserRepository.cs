using Bank.Models;

namespace Bank.Interfaces
{
    public interface IUserRepository
    {
        Task<List<Account>> GetUserAccounts();
        Task<Account> GetAccountById();
        Task<List<Transaction>> GetTransactionsOfAnUser();

    }
}
