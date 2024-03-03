using Bank.Data;
using Bank.Dtos.Account;
using Bank.Interfaces;
using Bank.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Bank.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;
        private readonly ICacheService _cacheService;
        private readonly ILogger<AccountRepository> _logger;

        public AccountRepository(DataContext context, ICacheService cacheService, ILogger<AccountRepository> logger)
        {
            _context = context;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<Account> CreateAccount(string userId)
        {
            var user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            var today = DateTime.Today;
            var age = today.Year - user.DateOfBirth.Year;
            
            if (user.DateOfBirth.Date > today.AddYears(-age)) age--;

            if (age < 18)
            {
                throw new Exception("You must be over 18 years to create an account.");
            }


            var newAccount = new Account
            {
                AppUserId = userId,
                AppUser = user,
                Balance = 0
            };

            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();
            //  _logger.LogInformation("Created account for {Name} with Id: {id}", newAccount.AppUser.UserName, newAccount.AccountId);
            Log.Information("Created new account for {Name} with Id: {id}", newAccount.AppUser.UserName, newAccount.AccountId);

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
            var cacheData = _cacheService.GetData<List<Account>>("accounts");

            if(cacheData != null && cacheData.Count() > 0)
            {
                return cacheData;
            }

            cacheData = await _context.Accounts.Include(a => a.AppUser).ToListAsync();

            var expiryTime = DateTimeOffset.Now.AddMinutes(5);

            _cacheService.SetData<List<Account>>("accounts", cacheData, expiryTime);

            return cacheData;

            // in memory cache
            //var accounts = await _cacheService.GetOrSetAsync<List<Account>>(
            //"accountsCacheKey",
            //async () => await _context.Accounts.Include(a => a.AppUser).ToListAsync(),
            //TimeSpan.FromMinutes(5));

            //return accounts;
        }

        public async Task<List<Account>> GetAccountsForAnUser(string id)
        {
            var accounts = await _context.Accounts.Where(a => a.AppUserId == id)
                                                  .Include(a => a.AppUser)
                                                  .ToListAsync();


            return accounts;

        }

        public async Task<Account> GetAccountWithId(int id)
        {
            var account = await _context.Accounts.Where(a => a.AccountId == id).Include(a => a.AppUser).FirstOrDefaultAsync();

            return account;

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
