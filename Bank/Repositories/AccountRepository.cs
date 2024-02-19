using Bank.Data;
using Bank.Interfaces;
using Bank.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;

        public AccountRepository(DataContext context)
        {
            _context = context;
        }

    
        public async Task<List<Account>> GetAccounts()
        {
            var accounts = await _context.Accounts.ToListAsync();

            return accounts;
        }
    }
}
