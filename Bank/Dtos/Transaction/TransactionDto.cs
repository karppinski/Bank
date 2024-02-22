using Bank.Models;

public class TransactionDto
{
    public int TransactionId { get; set; }
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public DateTime TimeStamp { get; set; }
}
