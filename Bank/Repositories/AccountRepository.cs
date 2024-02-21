using Bank.Data;
using Bank.Dtos.Account;
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

        public async Task<Account> CreateAccount(string userId)
        {
            var newAccount = new Account
            {
                AppUserId = userId,
                Balance = 0
            };

            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();

            return newAccount;
        }

        public async Task DeleteAccountById(int id)
        {
            var accountToDelete = await _context.Accounts.FindAsync(id);
            if(accountToDelete == null)
            {
                throw new Exception("Account not found");
            }

            _context.Accounts.Remove(accountToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountsForUserId(string userId)
        {
            var accounts = await _context.Accounts
                          .Where(u => u.AppUserId == userId)
                          .ToListAsync();

            if(accounts != null && accounts.Count > 0)
            {
                _context.RemoveRange(accounts);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Account>> GetAccounts()
        {
            var accounts = await _context.Accounts.ToListAsync();

            return accounts;
        }

        public async Task<List<Account>> GetAccountsForAnUser(string id)
        {
            var accounts = await _context.Accounts.Where(a => a.AppUserId == id)
                                                  .Include(a => a.AppUser)
                                                  .ToListAsync();


            return accounts;

        }

        public async Task<Account> GetUserAccountWithId(string userId, int accountId)
        {
           var account = await _context.Accounts
                                        .Include(a => a.AppUser)
                                        .FirstOrDefaultAsync(a => a.AppUserId == userId && a.AccountId == accountId);

            return account;
        }
    }
}
