using Bank.Data;
using Bank.Dtos.Account;
using Bank.Interfaces;
using Bank.Mappers;
using Bank.Models;
using Microsoft.Identity.Client;
using System.Security.Principal;
using System.Transactions;
using Transaction = Bank.Models.Transaction;

namespace Bank.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DataContext _context;

        public TransactionRepository(DataContext context)
        {
            _context = context;
        }

   
        public async Task<Transaction> Deposit(DepositDto depositDto)
        {
            var account = await _context.Accounts.FindAsync(depositDto.AccountId);
            if (account == null) return null;

            account.Balance += depositDto.Amount;

            var depositLog = new Transaction
            {
                AccountId = depositDto.AccountId,
                Amount = depositDto.Amount,
                Type = TransactionType.Deposit,
                TimeStamp = DateTime.UtcNow
            };

            _context.Transactions.Add(depositLog);
            await _context.SaveChangesAsync();

            return depositLog;
            
                          
        }

        public async Task<Transaction> GetTransactionById(int transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null) return null;

            return transaction;
        }

        public Task<Transaction> Transfer(int fomAccountId, int toAccountId, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> Withdraw(int accountId, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
