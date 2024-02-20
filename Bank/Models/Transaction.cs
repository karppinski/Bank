using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;

    }

    public enum TransactionType
    {
        Deposit,
        Withdraw,
        Transfer
    }
}
