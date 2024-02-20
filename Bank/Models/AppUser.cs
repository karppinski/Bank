using Microsoft.AspNetCore.Identity;

namespace Bank.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        public List<Account> Accounts { get; set; } = new List<Account>(); 

    }
}
