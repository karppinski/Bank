using Bank.Interfaces;
using Bank.Models;

namespace Bank.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<Account> GetAccountById()
        {
            throw new NotImplementedException();
        }

        public Task<List<Transaction>> GetTransactionsOfAnUser()
        {
            throw new NotImplementedException();
        }

        public Task<List<Account>> GetUserAccounts()
        {
            throw new NotImplementedException();
        }
    }
}
