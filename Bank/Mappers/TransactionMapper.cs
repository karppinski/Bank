using Bank.Dtos.Transaction;
using Bank.Models;

namespace Bank.Mappers
{
    public static class TransactionMapper
    {
        public static TransactionDto ToTransactionDto(this Transaction transactionModel)
        {
            return new TransactionDto
            {
                AccountId = transactionModel.AccountId,
                Amount = transactionModel.Amount,
                Type = transactionModel.Type,
                TimeStamp = transactionModel.TimeStamp
            };
        }
    }
}
