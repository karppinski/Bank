using Bank.Models;

namespace Bank.Dtos.Transaction
{
    public class TransactionDto
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}
