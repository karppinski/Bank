using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}
