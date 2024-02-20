using Bank.Dtos.Account;
using Bank.Models;
namespace Bank.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> Deposit(DepositDto depositDto);
        Task<Transaction> Withdraw(WithdrawDto withdrawDto);
        Task<IEnumerable<Transaction>> Transfer(TransferDto transferDto);
        Task<Transaction> GetTransactionById(int transactionId);
    }
}
