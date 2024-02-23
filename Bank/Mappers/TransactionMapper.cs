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
        public static List<TransactionDto> ToTransactionDtos(this List<Transaction> transactions)
        {
            var transactionDtos = new List<TransactionDto>();

            foreach(Transaction t in transactions)
            {
                transactionDtos.Add(

               new TransactionDto
               {
                   AccountId = t.AccountId,
                   Amount = t.Amount,
                   Type = t.Type,
                   TimeStamp = t.TimeStamp
               });
            }
            return transactionDtos;

        }
    }
}
