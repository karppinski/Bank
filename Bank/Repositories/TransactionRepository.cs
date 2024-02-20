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
            if (account == null) throw new Exception("Account not found");

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
            if (transaction == null) throw new Exception("Transaction not found");

            return transaction;
        }

        public async Task<IEnumerable<Transaction>> Transfer(TransferDto transferDto)
        {
            var fromAcc = await _context.Accounts.FindAsync(transferDto.FromAccountId);
            var toAcc = await _context.Accounts.FindAsync(transferDto.ToAccountId);

            if(fromAcc == null || toAcc == null)
            {
                throw new Exception("One or both accounts are invalid");   
            }
    
            if (fromAcc.Balance < transferDto.Amount)
            {
                throw new Exception("Insufficient funds for the transfer");
            }
            var toTransaction = new Transaction
            {
                AccountId = transferDto.ToAccountId,
                Amount = transferDto.Amount,
                Type = TransactionType.Transfer,
                TimeStamp = DateTime.Now
            };

            var fromTransaction = new Transaction
            {
                AccountId = transferDto.FromAccountId,
                Amount = -transferDto.Amount,
                Type = TransactionType.Transfer,
                TimeStamp = DateTime.Now
            };

            fromAcc.Balance -= transferDto.Amount;
            toAcc.Balance += transferDto.Amount;

            _context.Transactions.Add(toTransaction);
            _context.Transactions.Add(fromTransaction);

            await _context.SaveChangesAsync();
            var transactions = new List<Transaction> {fromTransaction, toTransaction };

            return transactions;

        }

        public async Task<Transaction> Withdraw(WithdrawDto withdrawDto)
        {
            var account = await _context.Accounts.FindAsync(withdrawDto.AccountId);
            if (account == null) throw new Exception("Account not found");

            account.Balance -= withdrawDto.Amount;

            var withdrawLog = new Transaction
            {
                AccountId = withdrawDto.AccountId,
                Amount = withdrawDto.Amount,
                Type = TransactionType.Withdraw,
                TimeStamp = DateTime.Now
            };

            _context.Transactions.Add(withdrawLog);
            await _context.SaveChangesAsync();

            return withdrawLog;
        }
    }
}
