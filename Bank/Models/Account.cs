namespace Bank.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public decimal Balance { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
