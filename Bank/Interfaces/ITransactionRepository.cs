using Bank.Dtos.Account;
using Bank.Models;
namespace Bank.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> Deposit(DepositDto depositDto);
        Task<Transaction> Withdraw(int accountId, decimal amount);
        Task<Transaction> Transfer(int fomAccountId,int toAccountId, decimal amount);
        Task<Transaction> GetTransactionById(int transactionId);
    }
}
