namespace Bank.Dtos.Account;

public class AccountDto
{
    public int AccountId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}
