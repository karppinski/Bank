using Bank.Dtos.Transaction;
using Bank.Models;
namespace Bank.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllTransactions();
        Task<List<Transaction>> GetAllTransactionsForAnAccount(int accId);
        Task<List<Transaction>> GetAllTransactionsForAnUser(string userId);
        Task<Transaction> Deposit(DepositDto depositDto);
        Task<Transaction> Withdraw(WithdrawDto withdrawDto);
        Task<IEnumerable<Transaction>> Transfer(TransferDto transferDto);
        Task<Transaction> GetTransactionById(int transactionId);
        Task DeleteTransactionsByAccountId(int accountId);
        Task DeleteTransactionsByUserId(string userId);
    }
}
